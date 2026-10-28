using CinemaBookingSystem.Data;
using CinemaBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        AppDbContext context = new AppDbContext();

        [HttpGet]
        public IActionResult GetIndexView()
        {
            var bookings = context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Seat)
                .ToList();

            return View("Index", bookings);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var bookings = context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Seat)
                .ToList();

            return View(bookings);
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Booking booking = context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Seat)
                    .ThenInclude(s => s.CinemaHall)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View("Details", booking);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            LoadDropdowns();

            return View("Create");
        }

        public IActionResult Create()
        {
            LoadDropdowns();

            return View();
        }

        [HttpPost]
        public IActionResult AddNew(Booking bookingFromCreateView)
        {
            bool seatAlreadyBooked =
                context.Bookings.Any(b =>
                    b.ShowTimeId == bookingFromCreateView.ShowTimeId &&
                    b.SeatId == bookingFromCreateView.SeatId);

            if (seatAlreadyBooked)
            {
                ModelState.AddModelError(
                    "SeatId",
                    "This seat is already booked for this show time.");
            }

            if (ModelState.IsValid)
            {
                bookingFromCreateView.BookingDate = DateTime.Now;

                context.Bookings.Add(bookingFromCreateView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            LoadDropdowns();

            return View("Create", bookingFromCreateView);
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Booking booking =
                context.Bookings.FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            LoadDropdowns();

            return View("Edit", booking);
        }

        [HttpPost]
        public IActionResult EditCurrent(Booking bookingFromEditView)
        {
            bool seatAlreadyBooked =
                context.Bookings.Any(b =>
                    b.ShowTimeId == bookingFromEditView.ShowTimeId &&
                    b.SeatId == bookingFromEditView.SeatId &&
                    b.Id != bookingFromEditView.Id);

            if (seatAlreadyBooked)
            {
                ModelState.AddModelError(
                    "SeatId",
                    "This seat is already booked for this show time.");
            }

            if (ModelState.IsValid)
            {
                context.Bookings.Update(bookingFromEditView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            LoadDropdowns();

            return View("Edit", bookingFromEditView);
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Booking booking = context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.ShowTime)
                    .ThenInclude(s => s.Movie)
                .Include(b => b.Seat)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View("Delete", booking);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Booking booking = context.Bookings.Find(id);

            if (booking == null)
                return NotFound();

            context.Bookings.Remove(booking);
            context.SaveChanges();

            return RedirectToAction(nameof(GetIndexView));
        }

        private void LoadDropdowns()
        {
            ViewBag.AllCustomers =
                new SelectList(
                    context.Customers,
                    "Id",
                    "FullName");

            ViewBag.AllShowTimes =
                context.ShowTimes
                    .Include(s => s.Movie)
                    .OrderBy(s => s.Date)
                    .ThenBy(s => s.StartTime)
                    .Select(s => new
                    {
                        s.Id,
                        s.CinemaHallId,
                        Text =
                            s.Movie.Title + " - " +
                            s.Date.ToString("dd/MM/yyyy") + " " +
                            s.StartTime.ToString(@"hh\:mm")
                    })
                    .ToList();

            ViewBag.AllSeats =
                context.Seats
                    .Include(s => s.CinemaHall)
                    .OrderBy(s => s.CinemaHall.Name)
                    .ThenBy(s => s.SeatNumber)
                    .Select(s => new
                    {
                        s.Id,
                        s.CinemaHallId,
                        Text = s.SeatNumber + " (" + s.CinemaHall.Name + ")"
                    })
                    .ToList();
        }
    }
}