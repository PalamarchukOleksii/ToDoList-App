using API.Data;
using API.Dtos.UserTask;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly ApplicationDbContext Context;

        public UserTaskRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<UserTask> CreateAsync(UserTask task)
        {
            await Context.Tasks.AddAsync(task);
            await Context.SaveChangesAsync();
            return task;
        }

        public async Task<UserTask?> DeleteAsync(int id)
        {
            var result = await Context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                return null;
            }

            Context.Tasks.Remove(result);
            await Context.SaveChangesAsync();

            return result;
        }

        public async Task<UserTask?> GetByIdAsync(int id)
        {
            UserTask? result = await Context.Tasks.FirstOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<List<UserTask>> GetUsersTaskAsync(int userId, UserTaskQueryObject query)
        {
            IQueryable<UserTask> tasks = Context.Tasks.AsQueryable().Where(x => x.UserId == userId);
            if (query.ChooseIsDone)
            {
                tasks = tasks.Where(x => x.IsDone == query.IsDone);
            }

            if (query.StartDate != null)
            {
                tasks = tasks.Where(x => query.StartDate == x.StartDateTime.Date);
            }

            if (query.EndDate != null)
            {
                tasks = tasks.Where(x => query.EndDate == x.EndDateTime.Date);
            }

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy.Equals("StartDateTime"))
                {
                    tasks = query.IsDescending ? tasks.OrderByDescending(x => x.StartDateTime) : tasks.OrderBy(x => x.StartDateTime);
                }

                if (query.SortBy.Equals("EndDateTime"))
                {
                    tasks = query.IsDescending ? tasks.OrderByDescending(x => x.EndDateTime) : tasks.OrderBy(x => x.EndDateTime);
                }
            }

            return await tasks.ToListAsync();
        }

        public async Task<UserTask?> UpdateAsync(int id, UserTaskUpdateDto taskDto)
        {
            UserTask? result = await Context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                return null;
            }

            result.ShortInfo = taskDto.ShortInfo;
            result.FullInfo = taskDto.FullInfo;
            result.EndDateTime = taskDto.EndDateTime;
            result.IsDone = taskDto.IsDone;
            await Context.SaveChangesAsync();

            return result;
        }
    }
}