namespace TaskManagerMVC.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserEmail { get; set; }
    }
}
