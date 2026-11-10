
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ResultApiService _resultApiService;

        public ResultsController(ResultApiService resultApiService)
        {
            _resultApiService = resultApiService;
        }

        public async Task<IActionResult> Index()
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

            var results = await _resultApiService.GetMyResultsAsync(token);

            if (results == null)
            {
                ViewBag.ErrorMessage =
                    "Unable to load your results. Please try again.";
            }

            return View(results);
        }
    }
}