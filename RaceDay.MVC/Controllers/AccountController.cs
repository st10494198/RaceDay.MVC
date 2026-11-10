using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthApiService _authApiService;

        public AccountController(AuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        // Display login page.
        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();

            return View();
        }

        // Process login using RaceDay API.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Please enter your email and password.";
                return View();
            }

            try
            {
                email = email.Trim();

                var result = await _authApiService.LoginAsync(email, password);

                if (result == null ||
                    string.IsNullOrWhiteSpace(result.Token) ||
                    result.User == null)
                {
                    ViewBag.ErrorMessage = "Invalid email or password.";
                    return View();
                }

                HttpContext.Session.Clear();

                HttpContext.Session.SetString("JWToken", result.Token);
                HttpContext.Session.SetString("UserID", result.User.UserID.ToString());
                HttpContext.Session.SetString("FirstName", result.User.FirstName);
                HttpContext.Session.SetString("LastName", result.User.LastName);
                HttpContext.Session.SetString("Email", result.User.Email);
                HttpContext.Session.SetString("Role", result.User.Role);

                if (result.User.Role == "Organiser")
                {
                    return RedirectToAction("Index", "Organiser");
                }

                if (result.User.Role == "Participant")
                {
                    return RedirectToAction("Index", "Participant");
                }

                HttpContext.Session.Clear();

                ViewBag.ErrorMessage = "Your account has an unrecognised role.";
                return View();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // Log out.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }
    }
}