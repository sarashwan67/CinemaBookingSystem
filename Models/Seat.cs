using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CinemaBookingSystem.Models
{
    public class Seat
    {
        public int Id { get; set; }

        public string SeatNumber { get; set; }

        public int CinemaHallId { get; set; }

        [ValidateNever]
        public CinemaHall CinemaHall { get; set; }

        [ValidateNever]
        public List<Booking> Bookings { get; set; }
    }
}
