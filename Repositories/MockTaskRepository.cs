
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repositories
{
    public class MockTaskRepository : ITaskRepository
    {
        private static List<TaskItem> TaskItems = new List<TaskItem>
        {
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Fix Backend Bug",
                Description = "Resolve issue with user authentication failing intermittently.",
                DueDate = "2025-03-05",
                Priority = "high",
                Status = "ongoing",
                AssignTo = "andra"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Design Landing Page",
                Description = "Create a new UI design for the company's landing page.",
                DueDate = "2025-03-10",
                Priority = "medium",
                Status = "pending",
                AssignTo = "andra"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Refactor API Endpoints",
                Description = "Improve REST API structure for better maintainability.",
                DueDate = "2025-03-12",
                Priority = "high",
                Status = "ongoing",
                AssignTo = "andra"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Optimize Database Queries",
                Description = "Reduce query execution time by optimizing SQL indexes.",
                DueDate = "2025-03-15",
                Priority = "high",
                Status = "ongoing",
                AssignTo = "andra"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Implement JWT Authentication",
                Description = "Secure API endpoints using JWT-based authentication.",
                DueDate = "2025-03-18",
                Priority = "high",
                Status = "ongoing",
                AssignTo = "andra"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Write Unit Tests for Services",
                Description = "Increase test coverage for business logic services.",
                DueDate = "2025-03-20",
                Priority = "medium",
                Status = "pending",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Migrate to .NET 8",
                Description = "Upgrade the project from .NET 6 to .NET 8 for better performance.",
                DueDate = "2025-03-22",
                Priority = "high",
                Status = "ongoing",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Enhance Error Logging",
                Description = "Improve error logging mechanism with structured logs.",
                DueDate = "2025-03-25",
                Priority = "low",
                Status = "completed",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Set Up CI/CD Pipeline",
                Description = "Automate build and deployment using GitHub Actions.",
                DueDate = "2025-03-28",
                Priority = "high",
                Status = "ongoing",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Fix Mobile Responsiveness",
                Description = "Ensure UI components are fully responsive on all devices.",
                DueDate = "2025-04-01",
                Priority = "medium",
                Status = "ongoing",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Integrate Payment Gateway",
                Description = "Add support for Stripe and PayPal payments.",
                DueDate = "2025-04-05",
                Priority = "high",
                Status = "pending",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Improve Search Algorithm",
                Description = "Enhance search ranking logic for better user experience.",
                DueDate = "2025-04-07",
                Priority = "medium",
                Status = "ongoing",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Automate Backup System",
                Description = "Set up daily database backups and offsite storage.",
                DueDate = "2025-04-10",
                Priority = "high",
                Status = "ongoing",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Add Dark Mode",
                Description = "Implement a dark theme toggle for better UX.",
                DueDate = "2025-04-15",
                Priority = "low",
                Status = "completed",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Improve API Rate Limiting",
                Description = "Prevent abuse by implementing IP-based rate limiting.",
                DueDate = "2025-04-18",
                Priority = "high",
                Status = "pending",
                AssignTo = "budi"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Add Multi-language Support",
                Description = "Allow users to switch between multiple languages.",
                DueDate = "2025-04-20",
                Priority = "medium",
                Status = "ongoing",
                AssignTo = "siti"
            },
            new TaskItem
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Refactor Frontend Code",
                Description = "Clean up redundant React components and improve structure.",
                DueDate = "2025-04-25",
                Priority = "medium",
                Status = "completed",
                AssignTo = "siti"
            }
        };

        public (List<TaskItem>, DTOMeta) GetTasks(string priority, string status, string searchText, int pageNumber, int pageSize)
        {
            var query = TaskItems.AsQueryable();

            if (!string.IsNullOrEmpty(priority))
                query = query.Where(t => t.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(t => t.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                         t.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            int totalItems = query.Count(); 
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);  

            var taskItems = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            DTOMeta dtoMeta = new DTOMeta
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,  
                FetchedAt = DateTime.UtcNow.ToString("o")  
            };

            return (taskItems, dtoMeta);
        }

        public TaskItem GetTask(string taskId)
        {
            return TaskItems.FirstOrDefault(t => t.Id == taskId);
        }

        public List<TaskItem> GetTasksByUser(string userId)
        {
            return TaskItems.Where(t => t.AssignTo == userId).ToList();
        }

        public TaskItem InsertTask(TaskItem taskItem)
        {
            taskItem.Id=Guid.NewGuid().ToString();
            TaskItems.Add(taskItem);
            return taskItem;
        }

        public TaskItem UpdateTask(string taskId, TaskItem taskItem)
        {
            var task = TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null) return null;

            task.Title = taskItem.Title;
            task.Description = taskItem.Description;
            task.DueDate = taskItem.DueDate;
            task.Priority = taskItem.Priority;
            task.Status = taskItem.Status;
            task.AssignTo = taskItem.AssignTo;

            return task;
        }

        public string DeleteTask(string taskId)
        {
            var task = TaskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null) return "false";

            TaskItems.Remove(task);
            return "true";
        }



    }
}
