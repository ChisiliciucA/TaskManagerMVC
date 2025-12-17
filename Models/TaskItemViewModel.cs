namespace TaskManagerMVC.Models
{
    public class TaskItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; } = null!;
    }
}
