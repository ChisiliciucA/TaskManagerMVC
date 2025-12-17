using TaskManagerMVC.Models;

namespace TaskManagerMVC.Services
{
    public interface ITasksService
    {
        IEnumerable<TaskViewModel> GetAllTasks();
        TaskViewModel? GetTaskById(int id);
        string ConvertTask(int userId, int taskId);

        //  schimbat din void în string
        string AddTask(TaskViewModel model);

        void UpdateTask(TaskViewModel model);
        void DeleteTask(int id);
    }
}
