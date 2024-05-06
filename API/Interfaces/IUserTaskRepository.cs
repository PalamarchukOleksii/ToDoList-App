using API.Dtos.UserTask;
using API.Helpers;
using API.Models;

namespace API.Interfaces
{
    public interface IUserTaskRepository
    {
        public Task<UserTask?> GetByIdAsync(int id);
        public Task<List<UserTask>> GetUsersTaskAsync(int userId, UserTaskQueryObject query);
        public Task<UserTask?> DeleteAsync(int id);
        public Task<UserTask?> UpdateAsync(int id, UserTaskUpdateDto taskDto);
        public Task<UserTask> CreateAsync(UserTask task);
    }
}
