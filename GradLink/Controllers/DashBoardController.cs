using GradLink.Model.ViewModel.QuestionAnswer;
using GradLink.Repository.MSSQL.ORM.Context;
using GradLink.Repository.MSSQL.ORM.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly GradLinkDbContext _db;

        public DashBoardController(GradLinkDbContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index() => View();

        public IActionResult Members() => View();

        public IActionResult Announcements() => View();

        public IActionResult Resources() => View();

        public IActionResult QNA()
        {
            var questions = _db.Questions
                .Include(q => q.Answers)
                .OrderByDescending(q => q.CreatedOn)
                .Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    UserName = q.UserName,
                    QuestionText = q.QuestionText,
                    CreatedOn = q.CreatedOn,
                    Answers = q.Answers
                        .OrderBy(a => a.CreatedOn)
                        .Select(a => new AnswerViewModel
                        {
                            Id = a.Id,
                            AnsweredBy = a.AnsweredBy,
                            AnswerText = a.AnswerText,
                            CreatedOn = a.CreatedOn
                        }).ToList()
                }).ToList();

            return View(questions);
        }

        public IActionResult Ask() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ask(string questionText)
        {
            if (string.IsNullOrWhiteSpace(questionText))
            {
                TempData["ErrorMessage"] = "Question cannot be empty.";
                return RedirectToAction("QNA");
            }

            var question = new Question
            {
                UserName = User.Identity?.Name,
                QuestionText = questionText.Trim(),
                CreatedOn = DateTime.Now
            };

            _db.Questions.Add(question);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Your question has been submitted.";
            return RedirectToAction("QNA");
        }

        public IActionResult Answer(int id)
        {
            var question = _db.Questions.Find(id);
            if (question == null) return NotFound();

            ViewBag.QuestionText = question.QuestionText;
            ViewBag.QuestionId = question.Id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Answer(int questionId, string answerText)
        {
            if (string.IsNullOrWhiteSpace(answerText))
            {
                TempData["ErrorMessage"] = "Answer cannot be empty.";
                return RedirectToAction("QNA");
            }

            var answer = new Answer
            {
                QuestionId = questionId,
                AnswerText = answerText.Trim(),
                AnsweredBy = User.Identity?.Name,
                CreatedOn = DateTime.Now
            };

            _db.Answers.Add(answer);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Answer posted successfully!";
            return RedirectToAction("QNA");
        }

        public IActionResult Career()
        {
            var adviceList = _db.CareerAdvices
                .OrderByDescending(c => c.CreatedDate)
                .ToList();

            return View(adviceList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CareerAdvice(CareerAdvice advice)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(advice.Title) || string.IsNullOrWhiteSpace(advice.Description))
            {
                TempData["ErrorMessage"] = "Both title and description are required.";
                return RedirectToAction("Career");
            }

            advice.Title = advice.Title.Trim();
            advice.Description = advice.Description.Trim();
            advice.CreatedDate = DateTime.Now;

            _db.CareerAdvices.Add(advice);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Career advice posted successfully!";
            return RedirectToAction("Career");
        }
    }
}
