using API.Dtos.User;
using API.Models;

namespace API.Mapper
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Tasks = user.Tasks.Select(c => c.ToTaskDto()).ToList()
            };
        }
    }
}