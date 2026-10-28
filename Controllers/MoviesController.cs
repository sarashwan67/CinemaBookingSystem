using CinemaBookingSystem.Data;
using CinemaBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace CinemaBookingSystem.Controllers
{
    public class MoviesController : Controller
    {
        AppDbContext context = new AppDbContext();
        IWebHostEnvironment webHostEnvironment;

        public MoviesController(IWebHostEnvironment webHostEnv)
        {
            webHostEnvironment = webHostEnv;
        }

        [HttpGet]
        public IActionResult GetIndexView(
            string? search,
            string? sortType,
            string? sortOrder)
        {
            ViewBag.CurrentSearch = search;

            IQueryable<Movie> movies =
                context.Movies.AsQueryable();

            // Search
            if (string.IsNullOrWhiteSpace(search) == false)
            {
                movies = movies.Where(m =>
                    m.Title.Contains(search) ||
                    m.Genre.Contains(search));
            }

            // Sort
            if (string.IsNullOrWhiteSpace(sortType) == false)
            {
                if (sortType == "Title" && sortOrder == "asc")
                {
                    movies = movies.OrderBy(m => m.Title);
                }
                else if (sortType == "Title" && sortOrder == "desc")
                {
                    movies = movies.OrderByDescending(m => m.Title);
                }
                else if (sortType == "Genre" && sortOrder == "asc")
                {
                    movies = movies.OrderBy(m => m.Genre);
                }
                else if (sortType == "Genre" && sortOrder == "desc")
                {
                    movies = movies.OrderByDescending(m => m.Genre);
                }
                else if (sortType == "Duration" && sortOrder == "asc")
                {
                    movies = movies.OrderBy(m => m.Duration);
                }
                else if (sortType == "Duration" && sortOrder == "desc")
                {
                    movies = movies.OrderByDescending(m => m.Duration);
                }
            }

            return View("Index", movies.ToList());
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(context.Movies.ToList());
        }


        // Details
        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Movie movie = context.Movies
                .Include(m => m.ShowTimes)
                .FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View("Details", movie);
        }


        // Create - GET
        [HttpGet]
        public IActionResult GetCreateView()
        {
            return View("Create");
        }


        public IActionResult Create()
        {
            return View();
        }


        // Create - POST
        [HttpPost]
        public IActionResult AddNew(Movie movieFromCreateView)
        {
            if (ModelState.IsValid)
            {
                if (movieFromCreateView.PosterFile == null)
                {
                    movieFromCreateView.PosterPath =
                        "\\images\\No_Image.png";
                }
                else
                {
                    Guid imageGuid = Guid.NewGuid();

                    string imageExtension =
                        Path.GetExtension(
                            movieFromCreateView.PosterFile.FileName);

                    string imageNewName =
                        imageGuid + imageExtension;

                    movieFromCreateView.PosterPath =
                        "\\images\\" + imageNewName;

                    string imageFullPath =
                        webHostEnvironment.WebRootPath +
                        movieFromCreateView.PosterPath;

                    FileStream imageFileStream =
                        new FileStream(
                            imageFullPath,
                            FileMode.Create);

                    movieFromCreateView.PosterFile.CopyTo(
                        imageFileStream);

                    imageFileStream.Dispose();
                }

                context.Movies.Add(movieFromCreateView);
                context.SaveChanges();

                return RedirectToAction(
                    nameof(GetIndexView));
            }

            return View(
                "Create",
                movieFromCreateView);
        }


        // Edit - GET
        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Movie movie =
                context.Movies.FirstOrDefault(
                    m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View("Edit", movie);
        }


        // Edit - POST
        [HttpPost]
        public IActionResult EditCurrent(
            Movie movieFromEditView)
        {
            if (ModelState.IsValid)
            {
                if (movieFromEditView.PosterFile != null)
                {
                    if (movieFromEditView.PosterPath !=
                        "\\images\\No_Image.png")
                    {
                        string oldImageFullPath =
                            webHostEnvironment.WebRootPath +
                            movieFromEditView.PosterPath;

                        System.IO.File.Delete(
                            oldImageFullPath);
                    }

                    Guid imageGuid = Guid.NewGuid();

                    string imageExtension =
                        Path.GetExtension(
                            movieFromEditView.PosterFile.FileName);

                    string imageNewName =
                        imageGuid + imageExtension;

                    movieFromEditView.PosterPath =
                        "\\images\\" + imageNewName;

                    string imageFullPath =
                        webHostEnvironment.WebRootPath +
                        movieFromEditView.PosterPath;

                    FileStream imageFileStream =
                        new FileStream(
                            imageFullPath,
                            FileMode.Create);

                    movieFromEditView.PosterFile.CopyTo(
                        imageFileStream);

                    imageFileStream.Dispose();
                }

                context.Movies.Update(movieFromEditView);
                context.SaveChanges();

                return RedirectToAction(
                    nameof(GetIndexView));
            }

            return View(
                "Edit",
                movieFromEditView);
        }


        // Delete - GET
        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Movie movie =
                context.Movies
                .FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View("Delete", movie);
        }


        // Delete - POST
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Movie movie =
                context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            if (movie.PosterPath !=
                "\\images\\No_Image.png")
            {
                string imageFullPath =
                    webHostEnvironment.WebRootPath +
                    movie.PosterPath;

                System.IO.File.Delete(
                    imageFullPath);
            }

            context.Movies.Remove(movie);
            context.SaveChanges();

            return RedirectToAction(
                nameof(GetIndexView));
        }
    }
}