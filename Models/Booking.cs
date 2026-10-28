using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CinemaBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public DateTime BookingDate { get; set; }

        public int CustomerId { get; set; }

        [ValidateNever]
        public Customer Customer { get; set; }

        public int ShowTimeId { get; set; }

        [ValidateNever]
        public ShowTime ShowTime { get; set; }

        public int SeatId { get; set; }

        [ValidateNever]
        public Seat Seat { get; set; }
    }
}
