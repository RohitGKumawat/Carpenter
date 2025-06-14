using System.ComponentModel.DataAnnotations;

namespace Carpenter.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)] // Ensures password has at least 6 characters
        public string Password { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; } = "Earth";  // Default country

        public string Photo { get; set; } = "/Media/Images/UserProfile/defaultuser.png"; // Default photo path

        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }

    }
}
