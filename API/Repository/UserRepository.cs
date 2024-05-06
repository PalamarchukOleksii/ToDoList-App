using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext Context;
        public UserRepository(ApplicationDbContext context)
        {
            Context = context;
        }
        public async Task<User?> GetUserAsync(string username)
        {
            User? result = await Context.Users.Include(i => i.Tasks).FirstOrDefaultAsync(x => x.UserName == username);
            if (result == null)
            {
                return null;
            }

            return result;
        }
    }
}