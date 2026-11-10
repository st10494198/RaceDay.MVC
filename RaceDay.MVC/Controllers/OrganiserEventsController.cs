
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserEventsController : Controller
    {
        private readonly OrganiserEventApiService _eventService;

        public OrganiserEventsController(OrganiserEventApiService eventService)
        {
            _eventService = eventService;
        }

        // VIEW ALL EVENTS
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var events = await _eventService.GetEventsAsync();

            return View(events);
        }

        // CREATE EVENT - GET
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            return View();
        }

        // CREATE EVENT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string EventName,
            DateTime EventDate,
            string Location,
            string Description)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(EventName) ||
                string.IsNullOrWhiteSpace(Location) ||
                string.IsNullOrWhiteSpace(Description) ||
                EventDate == DateTime.MinValue)
            {
                ViewBag.ErrorMessage = "Please complete all event fields correctly.";
                return View();
            }

            var eventData = new
            {
                eventName = EventName.Trim(),
                eventDate = EventDate,
                location = Location.Trim(),
                description = Description.Trim()
            };

            try
            {
                var success = await _eventService.CreateEventAsync(token, eventData);

                if (success)
                {
                    TempData["SuccessMessage"] = "Event created successfully!";
                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMessage =
                    "API rejected the event: " + _eventService.LastError;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage =
                    "Could not contact the API: " + ex.Message;

                return View();
            }
        }

        // EDIT EVENT - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var eventItem = await _eventService.GetEventAsync(id);

            if (!eventItem.HasValue)
                return NotFound();

            return View(eventItem.Value);
        }

        // EDIT EVENT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int EventID,
            string EventName,
            DateTime EventDate,
            string Location,
            string Description)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(EventName) ||
                string.IsNullOrWhiteSpace(Location) ||
                string.IsNullOrWhiteSpace(Description) ||
                EventDate == DateTime.MinValue)
            {
                TempData["ErrorMessage"] = "Please complete all event fields.";
                return RedirectToAction("Edit", new { id = EventID });
            }

            var eventData = new
            {
                eventID = EventID,
                eventName = EventName.Trim(),
                eventDate = EventDate,
                location = Location.Trim(),
                description = Description.Trim(),
                organiserID = int.Parse(
                    HttpContext.Session.GetString("UserID") ?? "0")
            };

            try
            {
                var success = await _eventService.UpdateEventAsync(
                    token, EventID, eventData);

                if (success)
                    TempData["SuccessMessage"] = "Event updated successfully!";
                else
                    TempData["ErrorMessage"] =
                        "Could not update event. " + _eventService.LastError;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Update failed: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // DELETE EVENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return RedirectToAction("Login", "Account");

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Login", "Account");

            try
            {
                var success = await _eventService.DeleteEventAsync(token, id);

                if (success)
                    TempData["SuccessMessage"] = "Event deleted successfully!";
                else
                    TempData["ErrorMessage"] =
                        "Could not delete event. " + _eventService.LastError;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Delete failed: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
