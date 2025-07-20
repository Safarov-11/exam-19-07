namespace Domain.Paginations;

public class ValidFilter
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }

    public ValidFilter()
    {

    }
    public ValidFilter(int pageSize, int pageNumber)
    {
        PageSize = pageSize < 1 ? 10 : pageSize;
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
    }
}
