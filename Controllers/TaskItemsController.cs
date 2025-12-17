using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.Data;         
using TaskManagerMVC.Models;       
using Microsoft.AspNetCore.Mvc.Rendering; 



public class TaskItemsController : Controller
{
    private readonly DatabaseContext _context;

    public TaskItemsController(DatabaseContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var tasks = _context.TaskItems.Include(t => t.User)
            .Select(t => new TaskItemViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                UserName = t.User.Name
            }).ToList();

        return View(tasks);
    }

  public IActionResult Create()
{
    var users = _context.Users.ToList();
    users.Insert(0, new User { Id = 0, Name = "" }); 
    ViewBag.Users = new SelectList(users, "Id", "Name");
    return View();
}


    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        if (ModelState.IsValid)
        {
            task.CreatedAt = DateTime.Now;
            _context.TaskItems.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewBag.Users = new SelectList(_context.Users, "Id", "Name", task.UserId);
        return View(task);
    }
}
