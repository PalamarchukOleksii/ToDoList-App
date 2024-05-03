using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using API.Dtos.User;
using API.Extensions;
using API.Interfaces;
using API.Mapper;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Contollers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> UserManager;
        private readonly ITokenService TokenService;
        private readonly SignInManager<User> SignInManager;
        public UserController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            TokenService = tokenService;
            SignInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(userDto.Username))
                {
                    return BadRequest("Username is empty");
                }

                if (string.IsNullOrEmpty(userDto.Password))
                {
                    return BadRequest("Password is empty");
                }

                User? user = await UserManager.Users.FirstOrDefaultAsync(x => x.UserName == userDto.Username.ToLower());
                if (user == null)
                {
                    return Unauthorized("Invalid username!");
                }

                var result = await SignInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
                if (!result.Succeeded)
                {
                    return Unauthorized("Username not found and/or password incorrect");
                }

                return Ok(new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = TokenService.CreateToken(user)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(userDto.Username))
                {
                    return BadRequest("Username is empty");
                }

                if (string.IsNullOrEmpty(userDto.Email))
                {
                    return BadRequest("Email is empty");
                }

                if (string.IsNullOrEmpty(userDto.Password))
                {
                    return BadRequest("Password is empty");
                }

                User user = new User
                {
                    UserName = userDto.Username,
                    Email = userDto.Email
                };

                IdentityResult createdUser = await UserManager.CreateAsync(user, userDto.Password);
                if (!createdUser.Succeeded)
                {
                    return BadRequest(createdUser.Errors);
                }

                return Ok(new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = TokenService.CreateToken(user)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? username = User.GetUsername();
            if (username == null)
            {
                return NotFound();
            }

            User? user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            string? tempUsername = user.UserName;
            string? tempEmail = user.Email;

            user.UserName = userDto.Username;
            user.Email = userDto.Email;
            IdentityResult result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                user.UserName = tempUsername;
                user.Email = tempEmail;
                return BadRequest();
            }

            return Ok(user.ToUserDto());
        }
    }
}