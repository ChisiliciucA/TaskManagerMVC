using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerMVC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

       public int ConversionCount { get; set; } = 0;

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
