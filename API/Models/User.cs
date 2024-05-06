using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class User : IdentityUser<int>
    {
        public List<UserTask> Tasks { get; set; } = new List<UserTask>();
    }
}