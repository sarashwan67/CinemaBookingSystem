using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CinemaBookingSystem.Models
{
    public class CinemaHall
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        [ValidateNever]
        public List<Seat> Seats { get; set; }

        [ValidateNever]
        public List<ShowTime> ShowTimes { get; set; }
    }
}
