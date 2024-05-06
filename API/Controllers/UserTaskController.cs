using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.Data;
using API.Helpers;
using Microsoft.AspNetCore.Identity;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using API.Dtos.UserTask;
using API.Mapper;
using API.Extensions;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [Route("api/usertask")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly IUserTaskRepository UserTaskRepository;
        private readonly ApplicationDbContext Context;
        private readonly IUserRepository UserRepository;

        public UserTaskController(IUserTaskRepository userTaskRepository, ApplicationDbContext context, IUserRepository userRepository)
        {
            UserTaskRepository = userTaskRepository;
            Context = context;
            UserRepository = userRepository;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserTask? result = await UserTaskRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.ToTaskDto());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] UserTaskQueryObject query)
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

            List<UserTask> result = await UserTaskRepository.GetUsersTaskAsync(user.Id, query);
            return Ok(result.Select(i => i.ToTaskDto()));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] UserTaskCreateDto taskDto)
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

            UserTask? task = taskDto.ToTaskFromCreateDto(user.Id);
            await UserTaskRepository.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task.ToTaskDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserTask? task = await UserTaskRepository.DeleteAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserTaskUpdateDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserTask? result = await UserTaskRepository.UpdateAsync(id, taskDto);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result.ToTaskDto());
        }
    }
}
