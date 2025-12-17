using TaskManagerMVC.Data;
using TaskManagerMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerMVC.Services
{
    public class TasksService : ITasksService
    {
        private readonly DatabaseContext _context;

        public TasksService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskViewModel> GetAllTasks()
        {
            return _context.Tasks.Include(t => t.User)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    UserEmail = t.User.Email
                }).ToList();
        }

        public TaskViewModel? GetTaskById(int id)
        {
            return _context.Tasks.Include(t => t.User)
                .Where(t => t.Id == id)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    UserEmail = t.User.Email
                }).FirstOrDefault();
        }

     public string AddTask(TaskViewModel model)
{
    var user = _context.Users.FirstOrDefault(u => u.Email == model.UserEmail);
    if (user == null)
    {
        user = new User
        {
            Email = model.UserEmail!,
            Name = model.UserEmail!.Split('@')[0],
            ConversionCount = 0
        };
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    if (user.ConversionCount >= 5)
    {
        return "Ai depășit numărul maxim de 5 conversii. Nu mai poți adăuga alte task-uri.";
    }

    user.ConversionCount++;
    _context.SaveChanges();

    var task = new TaskItem
    {
        Title = model.Title,
        Description = model.Description,
        IsCompleted = model.IsCompleted,
        CreatedAt = model.CreatedAt,
        UserId = user.Id
    };

    _context.Tasks.Add(task);
    _context.SaveChanges();

    return "Task adăugat cu succes!";
}

        public void UpdateTask(TaskViewModel model)
        {
            var task = _context.Tasks.Include(t => t.User).FirstOrDefault(t => t.Id == model.Id);
            if (task == null) return;

            var user = _context.Users.FirstOrDefault(u => u.Email == model.UserEmail);
            if (user == null)
            {
                user = new User
                {
                    Email = model.UserEmail!,
                    Name = model.UserEmail!.Split('@')[0]
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            task.Title = model.Title;
            task.Description = model.Description;
            task.IsCompleted = model.IsCompleted;
            task.CreatedAt = model.CreatedAt;
            task.UserId = user.Id;

            _context.SaveChanges();
        }

       public void DeleteTask(int id)
{
    var task = _context.Tasks.Include(t => t.User).FirstOrDefault(t => t.Id == id);
    if (task == null) return;

    var user = task.User;
    if (user != null && user.ConversionCount > 0)
    {
        
        user.ConversionCount--;
    }

    _context.Tasks.Remove(task);
    _context.SaveChanges();
}

public string ConvertTask(int userId, int taskId)
{
    var user = _context.Users.AsTracking().SingleOrDefault(u => u.Id == userId);
    if (user == null) return "Utilizator inexistent.";

    if (user.ConversionCount >= 5)
    {
        return "Ai depășit numărul maxim de 5 conversii. Nu mai poți efectua altele.";
    }

    var task = _context.Tasks.AsTracking().SingleOrDefault(t => t.Id == taskId);
    if (task == null) return "Task inexistent.";

    
    task.IsCompleted = true;

    
    user.ConversionCount++;

    _context.SaveChanges(); 

    return "Conversia a fost efectuată cu succes.";
}

    }
}
