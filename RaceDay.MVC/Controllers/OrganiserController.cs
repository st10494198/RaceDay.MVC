
using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            return View();
        }
    }
}
