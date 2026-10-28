using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CinemaBookingSystem.Models
{
    public class ShowTime
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public int MovieId { get; set; }

        [ValidateNever]
        public Movie Movie { get; set; }

        public int CinemaHallId { get; set; }

        [ValidateNever]
        public CinemaHall CinemaHall { get; set; }

        [ValidateNever]
        public List<Booking> Bookings { get; set; }
    }
}
