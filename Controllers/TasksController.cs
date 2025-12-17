using Microsoft.AspNetCore.Mvc;
using TaskManagerMVC.Models;
using TaskManagerMVC.Services; 

namespace TaskManagerMVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITasksService _tasksService;

        // Injectăm serviciul prin DI
        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        public IActionResult Index()
        {
            var tasks = _tasksService.GetAllTasks();
            return View(tasks);
        }

        public IActionResult Details(int id)
        {
            var task = _tasksService.GetTaskById(id);
            if (task == null) return NotFound();
            return View(task);
        }

        public IActionResult Create()
        {
            return View();
        }

       [HttpPost]
public IActionResult Create(TaskViewModel model)
{
    var result = _tasksService.AddTask(model);

    if (result.StartsWith("Ai depășit"))
    {
        ViewBag.Message = result;
        return View("LimitReached"); 
    }

    TempData["SuccessMessage"] = result;
    return RedirectToAction("Index");
}


        public IActionResult Edit(int id)
        {
            var task = _tasksService.GetTaskById(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                _tasksService.UpdateTask(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var task = _tasksService.GetTaskById(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _tasksService.DeleteTask(id);
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public IActionResult Convert(int taskId, int userId)
        {
            var message = _tasksService.ConvertTask(userId, taskId);

            if (message.Contains("depășit"))
                return BadRequest(message);

            return Ok(message);
        }
    }
}
