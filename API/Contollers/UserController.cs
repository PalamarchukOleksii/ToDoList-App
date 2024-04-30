using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos.User;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Contollers
{
    [Route("api/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> UserManager;
        public UserController(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Create([FromBody] UserRegisterDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if(string.IsNullOrEmpty(userDto.Password))
                {
                    return BadRequest("Password is empty");
                }

                User user = new User
                {
                    UserName = userDto.Username,
                    Email = userDto.Email
                };

                var createdUser = await UserManager.CreateAsync(user, userDto.Password);
                if(!createdUser.Succeeded)
                {
                    return BadRequest(createdUser.Errors);
                }

                return Ok(new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}