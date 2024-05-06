using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class UserRegisterDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Must be 8 symbol at least")]
        public string? Password { get; set; }
    }
}