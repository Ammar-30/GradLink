using GradLink.Model.ViewModel.Account;  // Imports view models related to user accounts (e.g., login, register forms)
using GradLink.Repository.MSSQL.ORM.Context;  // Provides access to the application's database context (DbContext for EF Core)
using GradLink.Repository.MSSQL.ORM.Entities; // Imports entity classes representing database tables (e.g., User, Post, etc.)
using Microsoft.AspNetCore.Authentication; // Provides classes for managing authentication (sign-in, sign-out, tokens, etc.)
using Microsoft.AspNetCore.Mvc; // Enables use of MVC features like controllers, actions, routing, and result types
using System.Security.Claims;   // Used for working with user identity and roles (e.g., ClaimsPrincipal, claim-based auth)


namespace GradLink.Controllers
{
    public class AccountController : Controller
    {
        private readonly GradLinkDbContext _dbContext;

        // Injecting database context through constructor
        public AccountController(GradLinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Default action - could redirect or display welcome/login page
        public IActionResult Index()
        {
            return View();
        }

        // GET: Show login form
        [HttpGet]
        public IActionResult Login()
        {
            // Redirect already logged-in users to main page
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Post");

            return View();
        }

        // POST: Handle login form submission
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // Validate form input
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check the form for errors.";
                return View(model);
            }

            // Check if user exists
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);

            // Verify password
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                // Fetch user's role
                var userRole = (from ur in _dbContext.UserRoles
                                join r in _dbContext.Roles on ur.RoleId equals r.RoleId
                                where ur.UserId == user.UserId
                                select r.RoleName).FirstOrDefault() ?? "User";

                // Create user claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, userRole)
                };

                // Create principal and sign in
                var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync("ApplicationCookie", principal);

                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Post");
            }

            // Login failed
            TempData["ErrorMessage"] = "Invalid email or password.";
            return View(model);
        }

        // GET: Show registration form
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Handle registration form submission
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check if email already exists
            var exists = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);
            if (exists != null)
            {
                TempData["ErrorMessage"] = "Email already exists.";
                return RedirectToAction("Register");
            }

            // Create new user and hash the password
            var user = new User
            {
                Username = model.UserName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedDate = DateTime.Now
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            // Assign default role to new user
            var defaultRole = _dbContext.Roles.FirstOrDefault(r => r.RoleName == "User");
            if (defaultRole != null)
            {
                _dbContext.UserRoles.Add(new UserRole
                {
                    UserId = user.UserId,
                    RoleId = defaultRole.RoleId
                });
                _dbContext.SaveChanges();
            }

            TempData["SuccessMessage"] = "Registration successful! You can now log in.";
            return RedirectToAction("Login");
        }

        // Log the user out and clear session
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync("ApplicationCookie");
            return RedirectToAction("Login");
        }

        // GET: Display user's profile
        public IActionResult Profile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _dbContext.Users.Find(userId);

            if (user == null)
                return NotFound();

            // Populate profile view model
            var model = new UserProfileViewModel
            {
                UserName = user.Username,
                Email = user.Email,
                Phone = user.PhoneNumber,
            };

            return View(model);
        }

        // POST: Handle profile updates
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _dbContext.Users.Find(userId);

            if (user == null)
                return NotFound();

            // Update user data
            user.Username = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            _dbContext.SaveChanges();

            // Re-sign the user with updated claims
            await HttpContext.SignOutAsync("ApplicationCookie");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "")
            };

            // Optional: Fetch user role again
            var userRole = (from ur in _dbContext.UserRoles
                            join r in _dbContext.Roles on ur.RoleId equals r.RoleId
                            where ur.UserId == user.UserId
                            select r.RoleName).FirstOrDefault() ?? "User";

            claims.Add(new Claim(ClaimTypes.Role, userRole));

            var identity = new ClaimsIdentity(claims, "ApplicationCookie");
            var principal = new ClaimsPrincipal(identity);
            HttpContext.SignInAsync("ApplicationCookie", principal);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }
    }
}
