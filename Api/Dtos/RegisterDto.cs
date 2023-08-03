using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        
        [Required]
        public string Username { get; set; }
       
        [Required]
        public string Role { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "SSN must be exactly 14 digits.")]
        public string SSN { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }
       
        [Required]
        public string State { get; set; }
       
        [Required]
        public string Country { get; set; }
       
        [Required]
        public int PostalCode { get; set; }

        [Required]
        //[RegularExpression(@"^(\+?1\s*-?)?\(?\d{3}\)?\s*-?\d{3}\s*-?\d{4}$", ErrorMessage = "Invalid phone number.")]

        public string PhoneNumber { get; set; }

    }
}
