using CinemaBookingSystem.Data;
using CinemaBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Controllers
{
    public class CinemaHallsController : Controller
    {
        AppDbContext context = new AppDbContext();

        [HttpGet]
        public IActionResult GetIndexView(string? search, string? sortType, string? sortOrder)
        {
            ViewBag.CurrentSearch = search;

            IQueryable<CinemaHall> halls =
                context.CinemaHalls.AsQueryable();

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                halls = halls.Where(h =>
                    h.Name.Contains(search));
            }

            if (string.IsNullOrWhiteSpace(sortType) == false)
            {
                if (sortType == "Name" && sortOrder == "asc")
                    halls = halls.OrderBy(h => h.Name);

                else if (sortType == "Name" && sortOrder == "desc")
                    halls = halls.OrderByDescending(h => h.Name);

                else if (sortType == "Capacity" && sortOrder == "asc")
                    halls = halls.OrderBy(h => h.Capacity);

                else if (sortType == "Capacity" && sortOrder == "desc")
                    halls = halls.OrderByDescending(h => h.Capacity);
            }

            return View("Index", halls.ToList());
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(context.CinemaHalls.ToList());
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            CinemaHall hall = context.CinemaHalls
                .Include(h => h.Seats)
                .Include(h => h.ShowTimes)
                .FirstOrDefault(h => h.Id == id);

            if (hall == null)
                return NotFound();

            return View("Details", hall);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            return View("Create");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNew(CinemaHall hallFromCreateView)
        {
            if (ModelState.IsValid)
            {
                context.CinemaHalls.Add(hallFromCreateView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            return View("Create", hallFromCreateView);
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            CinemaHall hall = context.CinemaHalls
                .FirstOrDefault(h => h.Id == id);

            if (hall == null)
                return NotFound();

            return View("Edit", hall);
        }

        [HttpPost]
        public IActionResult EditCurrent(CinemaHall hallFromEditView)
        {
            if (ModelState.IsValid)
            {
                context.CinemaHalls.Update(hallFromEditView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            return View("Edit", hallFromEditView);
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            CinemaHall hall = context.CinemaHalls
                .FirstOrDefault(h => h.Id == id);

            if (hall == null)
                return NotFound();

            return View("Delete", hall);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            CinemaHall hall = context.CinemaHalls.Find(id);

            if (hall == null)
                return NotFound();

            context.CinemaHalls.Remove(hall);
            context.SaveChanges();

            return RedirectToAction(nameof(GetIndexView));
        }
    }
}