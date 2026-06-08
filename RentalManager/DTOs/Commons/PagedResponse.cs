namespace RentalManager.DTOs.Commons
{
    public class PagedResponse<T>
    {
        public T Items { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public PagedResponse(T Item, int count, int pageNumber, int pageSize)
        {
            Items = Item;
            TotalRecords = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}
