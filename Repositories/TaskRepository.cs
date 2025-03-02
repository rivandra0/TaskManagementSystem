using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _jsonFilePath;

        public TaskRepository(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
        }

        #region helpers
        private class DBSchema
        {
            [JsonPropertyName("TaskItems")]
            public List<TaskItem> TaskItems { get; set; } = new();
        }

        private List<TaskItem> LoadTasksFromFile()
        {
            if (!File.Exists(_jsonFilePath))
            {
                return new List<TaskItem>();
            }

            string json = File.ReadAllText(_jsonFilePath);
            try
            {
                var data = JsonSerializer.Deserialize<DBSchema>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return data?.TaskItems ?? new List<TaskItem>();
            }
            catch (JsonException)
            {
                return new List<TaskItem>();
            }
        }

        private void SaveTasksToFile(List<TaskItem> taskItems)
        {
            var data = new DBSchema { TaskItems = taskItems };
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonFilePath, json);
        }

        #endregion

        public (List<TaskItem>, DTOMeta) GetTasks(string priority, string status, string searchText, int pageNumber, int pageSize)
        {
            var taskItems = LoadTasksFromFile().AsQueryable();

            if (!string.IsNullOrEmpty(priority))
                taskItems = taskItems.Where(t => t.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(status))
                taskItems = taskItems.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(searchText))
                taskItems = taskItems.Where(t => t.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                                 t.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase));

            int totalItems = taskItems.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedTasks = taskItems.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            DTOMeta dtoMeta = new DTOMeta
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                FetchedAt = DateTime.UtcNow.ToString("o")
            };

            return (paginatedTasks, dtoMeta);
        }

        public TaskItem GetTask(string taskId)
        {
            return LoadTasksFromFile().FirstOrDefault(t => t.Id == taskId);
        }

        public List<TaskItem> GetTasksByUser(string userId)
        {
            return LoadTasksFromFile().Where(t => t.AssignTo == userId).ToList();
        }

        public TaskItem InsertTask(TaskItem taskItem)
        {
            var taskItems = LoadTasksFromFile();
            taskItem.Id = Guid.NewGuid().ToString();
            taskItems.Add(taskItem);
            SaveTasksToFile(taskItems);
            return taskItem;
        }

        public TaskItem UpdateTask(string taskId, TaskItem updatedTask)
        {
            var taskItems = LoadTasksFromFile();
            var task = taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null) return null;

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.Priority = updatedTask.Priority;
            task.Status = updatedTask.Status;
            task.AssignTo = updatedTask.AssignTo;

            SaveTasksToFile(taskItems);
            return task;
        }

        public string DeleteTask(string taskId)
        {
            var taskItems = LoadTasksFromFile();
            var task = taskItems.FirstOrDefault(t => t.Id == taskId);
            if (task == null) return "false";

            taskItems.Remove(task);
            SaveTasksToFile(taskItems);
            return "true";
        }
    }
}
