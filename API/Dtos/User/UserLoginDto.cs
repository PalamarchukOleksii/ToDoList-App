using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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