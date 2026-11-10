using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventApiService _eventApiService;

        public EventsController(EventApiService eventApiService)
        {
            _eventApiService = eventApiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var events = await _eventApiService.GetEventsAsync();

                return View(events);
            }
            catch (HttpRequestException)
            {
                ViewBag.ErrorMessage =
                    "Unable to connect to the RaceDay API. Please make sure the API is running.";

                return View();
            }
        }
    }
}