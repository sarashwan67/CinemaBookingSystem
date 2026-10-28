using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "You have to provide a valid full name.")]
        [MinLength(12, ErrorMessage = "Full name mustn't be less than 12 characters.")]
        [MaxLength(50, ErrorMessage = "Full name mustn't exceed 50 characters.")]
        public string FullName { get; set; }
        [DisplayName("Phone")]
        [RegularExpression("^01\\d{9}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNo { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [NotMapped]
        [Compare(nameof(Email), ErrorMessage = "Email and ConfirmEmail do not match.")]
        [DisplayName("Confirm Email")]
        public string ConfirmEmail { get; set; }

        [ValidateNever]
        public List<Booking> Bookings { get; set; }

    }
}
