using CinemaBookingSystem.Data;
using CinemaBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Controllers
{
    public class CustomersController : Controller
    {
        AppDbContext context = new AppDbContext();

        [HttpGet]
        public IActionResult GetIndexView(
            string? search,
            string? sortType,
            string? sortOrder)
        {
            ViewBag.CurrentSearch = search;

            IQueryable<Customer> customers =
                context.Customers.AsQueryable();

            // Search
            if (string.IsNullOrWhiteSpace(search) == false)
            {
                customers = customers.Where(c =>
                    c.FullName.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.PhoneNo.Contains(search));
            }

            // Sort
            if (string.IsNullOrWhiteSpace(sortType) == false)
            {
                if (sortType == "FullName" && sortOrder == "asc")
                {
                    customers = customers.OrderBy(c => c.FullName);
                }
                else if (sortType == "FullName" && sortOrder == "desc")
                {
                    customers = customers.OrderByDescending(c => c.FullName);
                }
                else if (sortType == "Email" && sortOrder == "asc")
                {
                    customers = customers.OrderBy(c => c.Email);
                }
                else if (sortType == "Email" && sortOrder == "desc")
                {
                    customers = customers.OrderByDescending(c => c.Email);
                }
                else if (sortType == "PhoneNo" && sortOrder == "asc")
                {
                    customers = customers.OrderBy(c => c.PhoneNo);
                }
                else if (sortType == "PhoneNo" && sortOrder == "desc")
                {
                    customers = customers.OrderByDescending(c => c.PhoneNo);
                }
            }

            return View("Index", customers.ToList());
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(context.Customers.ToList());
        }


        // Details
        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Customer customer = context.Customers
                .Include(c => c.Bookings)
                    .ThenInclude(b => b.ShowTime)
                        .ThenInclude(s => s.Movie)
                .Include(c => c.Bookings)
                    .ThenInclude(b => b.Seat)
                .FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View("Details", customer);
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
        public IActionResult AddNew(Customer customerFromCreateView)
        {
            if (ModelState.IsValid)
            {
                context.Customers.Add(customerFromCreateView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            return View("Create", customerFromCreateView);
        }


        // Edit - GET
        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Customer customer =
                context.Customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View("Edit", customer);
        }


        // Edit - POST
        [HttpPost]
        public IActionResult EditCurrent(Customer customerFromEditView)
        {
            if (ModelState.IsValid)
            {
                context.Customers.Update(customerFromEditView);
                context.SaveChanges();

                return RedirectToAction(nameof(GetIndexView));
            }

            return View("Edit", customerFromEditView);
        }


        // Delete - GET
        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Customer customer =
                context.Customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View("Delete", customer);
        }


        // Delete - POST
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Customer customer = context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            context.Customers.Remove(customer);
            context.SaveChanges();

            return RedirectToAction(nameof(GetIndexView));
        }
    }
}