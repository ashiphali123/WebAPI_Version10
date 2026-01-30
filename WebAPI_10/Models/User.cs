namespace WebAPI_10.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Navigation property
        //public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
