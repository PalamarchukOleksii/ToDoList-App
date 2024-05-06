using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.UserTask
{
    public class UserTaskDto
    {
        public int Id { get; set; }
        public string? ShortInfo { get; set; }
        public string? FullInfo { get; set; }
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; }
        public bool IsDone { get; set; }
        public int UserId { get; set; }
    }
}