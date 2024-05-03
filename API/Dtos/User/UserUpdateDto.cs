using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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