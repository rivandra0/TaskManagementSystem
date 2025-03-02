using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Services;
using TaskManagementSystem.Models;
using System.Globalization;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly TaskService _taskItemService;

        public TaskController(ILogger<TaskController> logger, TaskService taskItemService)
        {
            _logger = logger;
            _taskItemService = taskItemService;
        }

        [HttpGet("task-items")]
        public ObjectResult GetTasks(string priority = "", string status = "", string searchText = "", int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                (List<TaskItem> taskItems, DTOMeta dtoMeta) = _taskItemService.GetTasks(priority, status, searchText, pageNumber, pageSize);

                DTO<IEnumerable<TaskItem>> dto = new()
                {
                    Message = "success",
                    Data = taskItems,
                    Meta = dtoMeta
                };
                return Ok(dto);
            }
            catch(AppHttpException htex) 
            {
                return StatusCode(htex.Code, new DTO<Object> { Message = htex.Message });
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }

        }


        [HttpGet("task-item/{id}")]
        public ObjectResult GetTask(string id)
        {
            try
            {
                DTO<TaskItem> dto = new()
                {
                    Message = "success",
                    Data = _taskItemService.GetTask(id)
                };
                return Ok(dto);
            }
            catch (AppHttpException htex)
            {
                return StatusCode(htex.Code, new DTO<Object> { Message = htex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }

        }


        [HttpGet("user/{userId}/task-items")]
        public ObjectResult GetTaskByUser(string userId)
        {
            try
            {
                DTO<List<TaskItem>> dto = new()
                {
                    Message = "success",
                    Data = _taskItemService.GetTasksByUser(userId)
                };
                return Ok(dto);
            }
            catch (AppHttpException htex)
            {
                return StatusCode(htex.Code, new DTO<Object> { Message = htex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }
            
        }


        [HttpPost("task-item")]
        public IActionResult InsertTask([FromBody] TaskItem taskItem)
        {
            try
            {
                _taskItemService.InsertTask(taskItem);

                return Ok(new DTO<TaskItem>
                {
                    Message = "Task inserted successfully",
                    Data = taskItem
                });
            }
            catch (AppHttpException htex)
            {
                return StatusCode(htex.Code, new DTO<Object> { Message = htex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }

        }


        [HttpPut("task-item/{id}")]
        public IActionResult UpdateTask(string id, [FromBody] TaskItem taskItem)
        {
            try
            {
                _taskItemService.UpdateTask(id, taskItem);

                return Ok(new DTO<TaskItem>
                {
                    Message = "Task updated successfully",
                    Data = taskItem
                });
            }
            catch (AppHttpException htex)
            {
                return StatusCode(htex.Code, new DTO<Object> { Message = htex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }
            
        }


        [HttpDelete("task-item/:id")]
        public ObjectResult DeleteTask(string id)
        {
            try
            {
                _taskItemService.DeleteTask(id);

                return Ok(new DTO<string>
                {
                    Message = "Task deleted successfully",
                    Data = id
                });
            }
            catch (AppHttpException htex)
            {
                return StatusCode(htex.Code, new DTO<Object> { Message = htex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred.");
            }
            
        }

    }
}
