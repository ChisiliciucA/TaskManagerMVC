using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.Data;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Controllers.Api
{
    [Route("api/Tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TasksApiController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Include(t => t.User)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    UserEmail = t.User.Email
                }).ToListAsync();

            return tasks;
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel>> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.User)
                .Where(t => t.Id == id)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    UserEmail = t.User.Email
                }).FirstOrDefaultAsync();

            if (task == null)
                return NotFound();

            return task;
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskViewModel>> PostTask(TaskViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.UserEmail);
            if (user == null)
            {
                user = new User
                {
                    Email = model.UserEmail!,
                    Name = model.UserEmail!.Split('@')[0]
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                IsCompleted = model.IsCompleted,
                CreatedAt = model.CreatedAt,
                UserId = user.Id
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            model.Id = task.Id;
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, model);
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskViewModel model)
        {
            var task = await _context.Tasks.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.UserEmail);
            if (user == null)
            {
                user = new User
                {
                    Email = model.UserEmail!,
                    Name = model.UserEmail!.Split('@')[0]
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            task.Title = model.Title;
            task.Description = model.Description;
            task.IsCompleted = model.IsCompleted;
            task.CreatedAt = model.CreatedAt;
            task.UserId = user.Id;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
