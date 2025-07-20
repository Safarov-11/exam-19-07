namespace Domain.ApiResponse;

public class PagedResponse<T> : Response<T>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public PagedResponse(T? data, int pageSize, int pageNumber, int totalRecords) : base(data)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (float) pageSize);
    }
}
