using Ayudantia.Src.Models;

namespace Ayudantia.Src.Dtos.Product;

public class ProductFiltersDto
{
    public required List<string> Brands { get; set; }
    public required List<string> Categories { get; set; }
    public required decimal MinPrice { get; set; }
    public required decimal MaxPrice { get; set; }
    public required List<ProductCondition> Conditions { get; set; }
}
