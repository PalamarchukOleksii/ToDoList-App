using API.Dtos.User;
using API.Extensions;
using API.Interfaces;
using API.Mapper;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IUserRepository UserRepository;
        public UserController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager, IUserRepository userRepository)
        {
            UserManager = userManager;
            TokenService = tokenService;
            SignInManager = signInManager;
            UserRepository = userRepository;
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
                    return Unauthorized("User with this name do not exist!");
                }

                var result = await SignInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
                if (!result.Succeeded)
                {
                    return Unauthorized("Password incorrect!");
                }

                return Ok(new NewUserDto
                {
                    Id = user.Id,
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
                    Id = user.Id,
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
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

            User? user = await UserRepository.GetUserAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToUserDto());
        }
    }
}
