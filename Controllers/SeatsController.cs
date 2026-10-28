using CinemaBookingSystem.Data;
using CinemaBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Controllers
{
    public class SeatsController : Controller
    {
        AppDbContext context = new AppDbContext();

        [HttpGet]
        public IActionResult GetIndexView(string? search)
        {
            ViewBag.CurrentSearch = search;

            IQueryable<Seat> seats = context.Seats
                .Include(s => s.CinemaHall);

            if (string.IsNullOrWhiteSpace(search) == false)
            {
                seats = seats.Where(s =>
                    s.SeatNumber.Contains(search) ||
                    s.CinemaHall.Name.Contains(search));
            }

            return View("Index", seats.ToList());
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(
                context.Seats
                    .Include(s => s.CinemaHall)
                    .ToList());
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Seat seat = context.Seats
                .Include(s => s.CinemaHall)
                .Include(s => s.Bookings)
                .FirstOrDefault(s => s.Id == id);

            if (seat == null)
                return NotFound();

            return View("Details", seat);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View("Create");
        }

        public IActionResult Create()
        {
            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View();
        }

        [HttpPost]
        public IActionResult AddNew(Seat seatFromCreateView)
        {
            if (ModelState.IsValid)
            {
                context.Seats.Add(seatFromCreateView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View("Create", seatFromCreateView);
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Seat seat = context.Seats
                .FirstOrDefault(s => s.Id == id);

            if (seat == null)
                return NotFound();

            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View("Edit", seat);
        }

        [HttpPost]
        public IActionResult EditCurrent(Seat seatFromEditView)
        {
            if (ModelState.IsValid)
            {
                context.Seats.Update(seatFromEditView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View("Edit", seatFromEditView);
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Seat seat = context.Seats
                .Include(s => s.CinemaHall)
                .FirstOrDefault(s => s.Id == id);

            if (seat == null)
                return NotFound();

            return View("Delete", seat);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Seat seat = context.Seats.Find(id);

            if (seat == null)
                return NotFound();

            context.Seats.Remove(seat);
            context.SaveChanges();

            return RedirectToAction(nameof(GetIndexView));
        }
    }
}