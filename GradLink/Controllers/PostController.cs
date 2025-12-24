using GradLink.Repository.MSSQL.ORM.Context;
using GradLink.Repository.MSSQL.ORM.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GradLink.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly GradLinkDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostController(GradLinkDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _db = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private long UserID =>
            long.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

        public IActionResult Index()
        {
            var posts = _db.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .OrderByDescending(p => p.CreatedOn)
                .ToList();

            return View(posts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(IFormFile imageFile, string textContent)
        {
            if (UserID == 0)
            {
                TempData["ErrorMessage"] = "User not identified.";
                return RedirectToAction("Index");
            }

            string imagePath = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","img", "Posts", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                imagePath = $"/img/Posts/{fileName}";
            }

            var post = new Post
            {
                TextContent = textContent,
                ImagePath = imagePath,
                UserId = (int)UserID,
                CreatedOn = DateTime.Now
            };

            _db.Posts.Add(post);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Post added successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(int postId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                TempData["ErrorMessage"] = "Comment cannot be empty.";
                return RedirectToAction("Index");
            }

            var comment = new Comment
            {
                PostId = postId,
                CommentText = commentText.Trim(),
                UserId = (int)UserID,
                CreatedOn = DateTime.Now
            };

            _db.Comments.Add(comment);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Comment added successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ToggleLike(int postId)
        {
            var like = _db.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == (int)UserID);

            if (like != null)
            {
                _db.Likes.Remove(like);
                _db.SaveChanges();

                var count = _db.Likes.Count(l => l.PostId == postId);
                return Json(new { success = true, likeCount = count, message = "You have unliked the post." });
            }

            _db.Likes.Add(new Like
            {
                PostId = postId,
                UserId = (int)UserID,
                CreatedOn = DateTime.Now
            });
            _db.SaveChanges();

            var updatedCount = _db.Likes.Count(l => l.PostId == postId);
            return Json(new { success = true, likeCount = updatedCount, message = "Thanks for liking the post!" });
        }

        [HttpGet]
        public JsonResult GetLikedUsers(int postId)
        {
            var users = _db.Likes
                .Where(l => l.PostId == postId)
                .Select(l => new
                {
                    l.User.UserId,
                    l.User.Username,
                    l.User.Email
                })
                .ToList();

            return Json(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int postId)
        {
            var post = _db.Posts
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.PostId == postId);

            if (post == null)
            {
                TempData["ErrorMessage"] = "Post not found.";
                return RedirectToAction("Index");
            }

            _db.Comments.RemoveRange(post.Comments);
            _db.Posts.Remove(post);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Post deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}