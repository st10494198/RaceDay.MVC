
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryApiService _categoryApiService;

        public CategoriesController(CategoryApiService categoryApiService)
        {
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var categories = await _categoryApiService.GetCategoriesAsync();

            if (categories == null)
            {
                ViewBag.ErrorMessage = "Unable to load categories. Please try again.";
            }

            return View(categories);
        }
    }
}

