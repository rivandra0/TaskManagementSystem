namespace TaskManagementSystem.Models
{
    public class TaskItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string DueDate { get; set; } = "";
        public string Priority { get; set; } = "";
        public string Status { get; set; } = "";
        public string AssignTo { get; set; } = "";
    }

    public static class TaskItemStatus
    {
        public const string Pending = "pending";
        public const string Ongoing = "ongoing";
        public const string Completed = "completed";

        public static bool IsValid(string value) =>
            value == Pending || value == Ongoing || value == Completed;
    }

    public static class TaskItemPriority
    {
        public const string Low = "low";
        public const string Medium = "medium";
        public const string High = "high";

        public static bool IsValid(string value) =>
            value == Low || value == Medium|| value == High;
    }

}
