
using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");

            return View();
        }
    }
}
