
namespace TaskManagementSystem.Models
{
    public class DTOMeta
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string FetchedAt { get; set; } = "";
    }

    public class DTO<T>
    {
        public string Message { get; set; } = "";
        public T? Data { get; set; } 
        public DTOMeta? Meta { get; set; } 
    }
}
