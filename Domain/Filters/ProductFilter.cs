using Domain.Paginations;

namespace Domain.Filters;

public class ProductFilter : ValidFilter
{
    public string? Name { get; set; }
    public int? StartPrice { get; set; }
    public int? ToPrice { get; set; }
}
