using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly EnrolmentApiService _enrolmentApiService;

        public EnrolmentsController(
            EnrolmentApiService enrolmentApiService)
        {
            _enrolmentApiService = enrolmentApiService;
        }

        [HttpGet]
        public IActionResult Create(int categoryId)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.CategoryId = categoryId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoryId, int raceNumber)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            if (raceNumber <= 0)
            {
                ViewBag.CategoryId = categoryId;
                ViewBag.ErrorMessage = "Please enter a valid race number.";
                return View();
            }

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                bool success = await _enrolmentApiService.CreateEnrolmentAsync(
                    token, categoryId, raceNumber);

                if (success)
                {
                    TempData["SuccessMessage"] =
                        "You have successfully entered this race category!";

                    return RedirectToAction("Index", "Categories");
                }

                ViewBag.CategoryId = categoryId;
                ViewBag.ErrorMessage =
                    "Your enrolment could not be completed. Please try again.";

                return View();
            }
            catch (HttpRequestException)
            {
                ViewBag.CategoryId = categoryId;
                ViewBag.ErrorMessage =
                    "Unable to connect to the RaceDay API.";

                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyEvents()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var enrolments =
                await _enrolmentApiService.GetMyEnrolmentsAsync(token);

            if (enrolments == null)
            {
                ViewBag.ErrorMessage =
                    "Unable to load your race entries. Please try again.";
            }

            return View(enrolments);
        }
    }
}