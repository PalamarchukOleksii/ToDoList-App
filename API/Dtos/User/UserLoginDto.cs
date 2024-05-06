using System.ComponentModel.DataAnnotations;

namespace API.Dtos.User
{
    public class UserLoginDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Must be 8 symbol at least")]
        public string? Password { get; set; }
    }
}