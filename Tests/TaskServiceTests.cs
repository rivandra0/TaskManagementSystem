using Moq;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories;
using TaskManagementSystem.Services;
using Xunit;

namespace TaskManagementSystem.Tests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepo;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _taskService = new TaskService(_mockRepo.Object);
    }

    [Fact]
    public void GetTasks_ShouldThrowException_WhenPageNumberIsInvalid()
    {
        var ex = Assert.Throws<AppHttpException>(() => _taskService.GetTasks("high", "open", "test", 0, 10));
        Assert.Contains("page number must be bigger than 0", ex.Message);
    }

    [Fact]
    public void GetTasks_ShouldReturnTasks_WhenValid()
    {
        var tasks = new List<TaskItem> { new TaskItem { Title = "Test Task" } };
        var meta = new DTOMeta();
        _mockRepo.Setup(r => r.GetTasks(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns((tasks, meta));

        var result = _taskService.GetTasks("high", "open", "test");

        Assert.NotNull(result);
        Assert.Single(result.Item1);
        Assert.Equal("Test Task", result.Item1[0].Title);
    }

    [Fact]
    public void GetTask_ShouldThrowException_WhenIdIsNull()
    {
        var ex = Assert.Throws<AppHttpException>(() => _taskService.GetTask(""));
        Assert.Contains("must include id", ex.Message);
    }

    [Fact]
    public void GetTask_ShouldReturnTask_WhenValidId()
    {
        var task = new TaskItem { Title = "Test Task" };
        _mockRepo.Setup(r => r.GetTask(It.IsAny<string>())).Returns(task);

        var result = _taskService.GetTask("1");

        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public void InsertTask_ShouldThrowException_WhenTitleIsMissing()
    {
        var task = new TaskItem { Priority = "high", Status = "ongoing", AssignTo = "andra", DueDate = "2025-05-01" };
        var ex = Assert.Throws<AppHttpException>(() => _taskService.InsertTask(task));
        Assert.Contains("must include title", ex.Message);
    }

    [Fact]
    public void InsertTask_ShouldReturnInsertedTask_WhenValid()
    {
        var task = new TaskItem { Title = "Test", Priority = "high", Status = "ongoing", AssignTo = "andra", DueDate = "2025-05-01" };
        _mockRepo.Setup(r => r.InsertTask(It.IsAny<TaskItem>())).Returns(task);

        var result = _taskService.InsertTask(task);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Title);
    }

    [Fact]
    public void UpdateTask_ShouldThrowException_WhenInvalidId()
    {
        var task = new TaskItem { Title = "Updated", Priority = "low", Status = "completed", AssignTo = "budi", DueDate = "2025-04-01" };
        var ex = Assert.Throws<AppHttpException>(() => _taskService.UpdateTask("", task));
        Assert.Contains("must include id", ex.Message);
    }

    [Fact]
    public void DeleteTask_ShouldThrowException_WhenIdIsMissing()
    {
        var ex = Assert.Throws<AppHttpException>(() => _taskService.DeleteTask(""));
        Assert.Contains("must include id", ex.Message);
    }

    [Fact]
    public void DeleteTask_ShouldReturnDeletedTaskId_WhenValid()
    {
        _mockRepo.Setup(r => r.DeleteTask(It.IsAny<string>())).Returns("1");

        var result = _taskService.DeleteTask("1");

        Assert.Equal("1", result);
    }
}
