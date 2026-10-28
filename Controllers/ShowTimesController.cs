using CinemaBookingSystem.Data;
using CinemaBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Controllers
{
    public class ShowTimesController : Controller
    {
        AppDbContext context = new AppDbContext();

        [HttpGet]
        public IActionResult GetIndexView()
        {
            var showTimes = context.ShowTimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .ToList();

            return View("Index", showTimes);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var showTimes = context.ShowTimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .ToList();

            return View(showTimes);
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            ShowTime showTime = context.ShowTimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .Include(s => s.Bookings)
                .FirstOrDefault(s => s.Id == id);

            if (showTime == null)
                return NotFound();

            return View("Details", showTime);
        }

        [HttpGet]
        public IActionResult GetCreateView()
        {
            ViewBag.AllMovies =
                new SelectList(
                    context.Movies,
                    "Id",
                    "Title");

            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View("Create");
        }

        public IActionResult Create()
        {
            ViewBag.AllMovies =
                new SelectList(
                    context.Movies,
                    "Id",
                    "Title");

            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View();
        }

        [HttpPost]
        public IActionResult AddNew(ShowTime showTimeFromCreateView)
        {
            Movie selectedMovie =
                context.Movies.FirstOrDefault(
                    m => m.Id == showTimeFromCreateView.MovieId);

            if (selectedMovie != null &&
                showTimeFromCreateView.Date.Date < selectedMovie.ReleaseDate.Date)
            {
                ModelState.AddModelError(
                    "Date",
                    "Show time date can't be before the movie's release date (" +
                    selectedMovie.ReleaseDate.ToShortDateString() + ").");
            }

            if (ModelState.IsValid)
            {
                context.ShowTimes.Add(showTimeFromCreateView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            ViewBag.AllMovies =
                new SelectList(
                    context.Movies,
                    "Id",
                    "Title");

            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View("Create", showTimeFromCreateView);
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            ShowTime showTime =
                context.ShowTimes.FirstOrDefault(s => s.Id == id);

            if (showTime == null)
                return NotFound();

            ViewBag.AllMovies =
                new SelectList(
                    context.Movies,
                    "Id",
                    "Title");

            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View("Edit", showTime);
        }

        [HttpPost]
        public IActionResult EditCurrent(ShowTime showTimeFromEditView)
        {
            Movie selectedMovie =
                context.Movies.FirstOrDefault(
                    m => m.Id == showTimeFromEditView.MovieId);

            if (selectedMovie != null &&
                showTimeFromEditView.Date.Date < selectedMovie.ReleaseDate.Date)
            {
                ModelState.AddModelError(
                    "Date",
                    "Show time date can't be before the movie's release date (" +
                    selectedMovie.ReleaseDate.ToShortDateString() + ").");
            }

            if (ModelState.IsValid)
            {
                context.ShowTimes.Update(showTimeFromEditView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            ViewBag.AllMovies =
                new SelectList(
                    context.Movies,
                    "Id",
                    "Title");

            ViewBag.AllCinemaHalls =
                new SelectList(
                    context.CinemaHalls,
                    "Id",
                    "Name");

            return View("Edit", showTimeFromEditView);
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            ShowTime showTime = context.ShowTimes
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .FirstOrDefault(s => s.Id == id);

            if (showTime == null)
                return NotFound();

            return View("Delete", showTime);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            ShowTime showTime = context.ShowTimes.Find(id);

            if (showTime == null)
                return NotFound();

            context.ShowTimes.Remove(showTime);
            context.SaveChanges();

            return RedirectToAction(nameof(GetIndexView));
        }
    }
}