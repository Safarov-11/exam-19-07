using Domain.ApiResponse;
using Microsoft.EntityFrameworkCore;

namespace Domain.Paginations;

public class Pagination<T>(IQueryable<T> queryable)
{
    public async Task<PagedResponse<List<T>>> GetPagedResponseAsync(int pageSize, int pageNumber)
    {
        try
        {
            var validFilter = new ValidFilter(pageSize, pageNumber);
            var query = queryable;

            var totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return new PagedResponse<List<T>>(data, pageSize, pageNumber, totalRecords);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
