using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repositories
{
    public interface ITaskRepository
    {
        (List<TaskItem>, DTOMeta) GetTasks(
            string priority,
            string status,
            string searchText,
            int pageNumber,
            int pageSize
        );
        TaskItem GetTask(string taskId);
        List<TaskItem> GetTasksByUser(string userId);
        TaskItem InsertTask(TaskItem taskItem);
        TaskItem UpdateTask(string taskId, TaskItem taskItem);
        string DeleteTask(string taskId); 
    }
}
