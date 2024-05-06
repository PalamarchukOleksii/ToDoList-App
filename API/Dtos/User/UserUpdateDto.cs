using System.ComponentModel.DataAnnotations;
namespace API.Dtos.User
{
    public class UserUpdateDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Username cannot be over 50 characters")]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}