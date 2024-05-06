using API.Dtos.UserTask;

namespace API.Dtos.User
{
    public class UserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<UserTaskDto> Tasks { get; set; } = new List<UserTaskDto>();
    }
}