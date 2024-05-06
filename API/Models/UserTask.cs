namespace API.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? ShortInfo { get; set; }
        public string? FullInfo { get; set; }
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; }
        public bool IsDone { get; set; } = false;
    }
}
