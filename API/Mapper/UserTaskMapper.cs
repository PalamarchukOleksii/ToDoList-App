using API.Dtos.UserTask;
using API.Models;

namespace API.Mapper
{
    public static class UserTaskMapper
    {
        public static UserTaskDto ToTaskDto(this UserTask task)
        {
            return new UserTaskDto
            {
                Id = task.Id,
                ShortInfo = task.ShortInfo,
                FullInfo = task.FullInfo,
                StartDateTime = task.StartDateTime,
                EndDateTime = task.EndDateTime,
                IsDone = task.IsDone,
                UserId = task.UserId
            };
        }

        public static UserTask ToTaskFromCreateDto(this UserTaskCreateDto taskDto, int userId)
        {
            return new UserTask
            {
                UserId = userId,
                ShortInfo = taskDto.ShortInfo,
                FullInfo = taskDto.FullInfo,
                EndDateTime = taskDto.EndDateTime
            };
        }
    }
}