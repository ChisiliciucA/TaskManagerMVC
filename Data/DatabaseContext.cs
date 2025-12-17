using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        public DbSet<TaskItem> TaskItems => Tasks; // ✅ alias suplimentar
    }
}
