using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.User
{
    public class NewUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}