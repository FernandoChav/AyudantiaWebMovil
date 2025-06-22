using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos.Product;
using Ayudantia.Src.Interfaces;
using Ayudantia.Src.Models;

using Microsoft.EntityFrameworkCore;

namespace Ayudantia.Src.Repositories;

public class ProductRepository(StoreContext store, ILogger<Product> logger) : IProductRepository
{
    private readonly StoreContext _context = store;
    private readonly ILogger<Product> _logger = logger;

    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public Task DeleteProductAsync(Product product)
    {
        _context.Products.Remove(product);
        return Task.CompletedTask;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id) ?? null;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public IQueryable<Product> GetQueryableProducts()
    {
        return _context.Products.AsQueryable();
    }

    public Task UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        return Task.CompletedTask;
    }

    public async Task<bool> IsProductInOrdersAsync(int productId)
    {
        return await _context.OrderItems.AnyAsync(i => i.ProductId == productId);
    }

    public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids)
    {
        return await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
    }

    public async Task<ProductFiltersDto> GetProductFiltersAsync()
    {
        List<string> products = await _context
            .Products.Select(x => x.Brand)
            .Distinct()
            .ToListAsync();
        List<string> categories = await _context
            .Products.Select(x => x.Category)
            .Distinct()
            .ToListAsync();
        decimal minPrice = (decimal)await _context.Products.MinAsync(x => (double)x.Price);
        decimal maxPrice = (decimal)await _context.Products.MaxAsync(x => (double)x.Price);
        List<ProductCondition> conditions = await _context
            .Products.Select(x => x.Condition)
            .Distinct()
            .ToListAsync();

        return new ProductFiltersDto
        {
            Brands = products,
            Categories = categories,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Conditions = conditions,
        };
    }
}
