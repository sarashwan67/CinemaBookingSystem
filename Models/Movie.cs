using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Models
{
    public class Movie
    {
      
            public int Id { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public int Duration { get; set; }

            public string Genre { get; set; }

            public DateTime ReleaseDate { get; set; }

        [ValidateNever]
        [NotMapped]
        [Display(Name = "Poster")]
        public IFormFile? PosterFile { get; set; }

        [ValidateNever]
        public string? PosterPath { get; set; }

        [ValidateNever]
        public List<ShowTime> ShowTimes { get; set; }


    }
}
