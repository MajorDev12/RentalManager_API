namespace RentalManager.DTOs.Commons
{
    public class QueryFilter
    {
        // Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Sorting
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;

        // General search
        public string? SearchTerm { get; set; }
    }
}
