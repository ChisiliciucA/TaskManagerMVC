using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerMVC.Models
{
    [Table("Tasks")] 
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

    

    }
}
