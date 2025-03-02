using System.Globalization;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories;

namespace TaskManagementSystem.Services
{
    public class TaskService
    {
        public ITaskRepository Repository { get; }

        public TaskService(ITaskRepository repo)
        {
            Repository = repo;
        }

        public (List<TaskItem>, DTOMeta) GetTasks(string priority, string status, string searchText, int pageNumber = 1, int pageSize = 10)
        {
            #region validation
            var err = new System.Text.StringBuilder();

            //if (string.IsNullOrWhiteSpace(priority)) { err.Append("must include priority param; "); }
            //if (string.IsNullOrWhiteSpace(status)) { err.Append("must include status param; "); }
            if (pageNumber < 1) { err.Append("page number must be bigger than 0; "); }
            if (pageSize < 1) { err.Append("page size must be bigger than 0; "); }

            if (err.Length > 0)
            {
                throw new AppHttpException(400, err.ToString().Trim());
            }

            #endregion
            return Repository.GetTasks(priority, status, searchText, pageNumber, pageSize);
        }

        public TaskItem GetTask(string id)
        {
            #region validation
            var err = new System.Text.StringBuilder();

            if (string.IsNullOrWhiteSpace(id)) { err.Append("must include id; "); }
            if (err.Length > 0)
            {
                throw new AppHttpException(400, err.ToString().Trim());
            }

            #endregion

            return Repository.GetTask(id);
        }

        public List<TaskItem> GetTasksByUser(string userId)
        {
            #region validation
            var err = new System.Text.StringBuilder();

            if (string.IsNullOrWhiteSpace(userId)) { err.Append("must include userId; "); }
            if (err.Length > 0)
            {
                throw new AppHttpException(400, err.ToString().Trim());
            }

            #endregion

            return Repository.GetTasksByUser(userId);
        }

        public TaskItem InsertTask(TaskItem taskItem)
        {
            #region validation
            var err = new System.Text.StringBuilder();

            if (string.IsNullOrWhiteSpace(taskItem.Title)) { err.Append("must include title; "); }
            if (string.IsNullOrWhiteSpace(taskItem.Priority)) { err.Append("must include priority; "); }
            if (!TaskItemPriority.IsValid(taskItem.Priority)) { err.Append("priority invalid; "); }

            if (string.IsNullOrWhiteSpace(taskItem.Status)) { err.Append("must include status; "); }
            if (!TaskItemStatus.IsValid(taskItem.Status)) { err.Append("status invalid; "); }

            if (string.IsNullOrWhiteSpace(taskItem.AssignTo)) { err.Append("must include assign to; "); }

            if (DateTime.TryParseExact(taskItem.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate))
            {
                if (dueDate < DateTime.Now.Date)
                {
                    err.Append("Due date cannot be in the past; ");
                }
            }
            else
            {
                err.Append("Invalid due date format. Use yyyy-MM-dd; ");
            }


            if (err.Length > 0)
            {
                throw new AppHttpException(400, err.ToString().Trim());
            
            }
            #endregion

            return Repository.InsertTask(taskItem);
        }

        public TaskItem UpdateTask(string id, TaskItem updatedTask)
        {
            #region validation
            var err = new System.Text.StringBuilder();

            if (string.IsNullOrWhiteSpace(updatedTask.Title)) { err.Append("must include title; "); }
            if (string.IsNullOrWhiteSpace(updatedTask.Priority)) { err.Append("must include priority; "); }
            if (!TaskItemPriority.IsValid(updatedTask.Priority)) { err.Append("priority invalid; "); }

            if (string.IsNullOrWhiteSpace(updatedTask.Status)) { err.Append("must include status; "); }
            if (!TaskItemStatus.IsValid(updatedTask.Status)) { err.Append("status invalid; "); }

            if (string.IsNullOrWhiteSpace(updatedTask.AssignTo)) { err.Append("must include assign to; "); }

            if (DateTime.TryParseExact(updatedTask.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate))
            {
                if (dueDate < DateTime.Now.Date)
                {
                    err.Append("Due date cannot be in the past; ");
                }
            }
            else
            {
                err.Append("Invalid due date format. Use yyyy-MM-dd; ");
            }


            if (err.Length > 0)
            {
                throw new AppHttpException(400, err.ToString().Trim());
            }
            #endregion
            return Repository.UpdateTask(id, updatedTask);
        }

        public string DeleteTask(string id)
        {
            #region validation
            var err = new System.Text.StringBuilder();

            if (string.IsNullOrWhiteSpace(id)) { err.Append("must include id; "); }
            if (err.Length > 0)
            {
                throw new AppHttpException(400, err.ToString().Trim());

            }
            #endregion

            return Repository.DeleteTask(id);
        }
    }
}
