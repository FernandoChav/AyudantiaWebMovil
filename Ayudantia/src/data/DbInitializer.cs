using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.src.models;

using Bogus;

using Microsoft.EntityFrameworkCore;

namespace Ayudantia.src.data
{
    public class DbInitializer
    {
        public static void InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<StoreContext>()
                ?? throw new InvalidOperationException("Could not get StoreContext");

            SeedData(context);
        }

        private static void SeedData(StoreContext context)
        {
            context.Database.Migrate();

            if (context.Products.Any()) return;

            var faker = new Faker("es");

            var products = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Category, f => f.Commerce.Department())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Decimal(5000, 50000))
                .RuleFor(p => p.Brand, f => f.Company.CompanyName())
                .RuleFor(p => p.Stock, f => f.Random.Int(10, 200))
                .RuleFor(p => p.Urls, f => new[]
                {
                    $"https://res.cloudinary.com/demo/image/upload/sample1.jpg",
                    $"https://res.cloudinary.com/demo/image/upload/sample2.jpg",
                    $"https://res.cloudinary.com/demo/image/upload/sample3.jpg"
                })
                .Generate(10);

            context.Set<Product>().AddRange(products);
            context.SaveChanges();

        }
    }
}