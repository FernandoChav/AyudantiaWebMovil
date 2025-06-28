using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Interfaces;
using Ayudantia.Src.Models;

using Bogus;

namespace Ayudantia.Src.Data.Seeders
{
    public class ProductSeeder
    {
        private static readonly List<string> Brands = new() { "TechZone", "EcoFashion", "UrbanGear", "HomePlus", "KiddoToys" };
        private static readonly List<string> Categories = new() { "Electronics", "Clothing", "Toys", "Home", "Books" };

        public static async Task<List<Product>> GenerateProductsAsync(int quantity, IPhotoService photoService)
        {
            var faker = new Faker("es");
            var random = new Random();
            var imageCache = new Dictionary<string, List<string>>();

            foreach (var category in Categories)
            {
                if (!imageCache.ContainsKey(category))
                    imageCache[category] = new();

                var localPath = Path.Combine("Assets", "Products", category);
                var imagePaths = Directory.GetFiles(localPath, "*.jpg");

                foreach (var imgPath in imagePaths)
                {
                    var uploadResult = await photoService.AddPhotoFromPathAsync(imgPath);
                    if (uploadResult.Error == null)
                    {
                        imageCache[category].Add(uploadResult.SecureUrl.AbsoluteUri);
                    }
                }
            }

            var products = new List<Product>();

            for (int i = 0; i < quantity; i++)
            {
                var category = faker.PickRandom(Categories);
                var brand = faker.PickRandom(Brands);
                var urls = imageCache[category].OrderBy(_ => Guid.NewGuid()).Take(3).ToList();

                products.Add(new Product
                {
                    Name = faker.Commerce.ProductName(),
                    Description = faker.Commerce.ProductDescription(),
                    Category = category,
                    Brand = brand,
                    Price = faker.Random.Decimal(5000, 50000),
                    Stock = faker.Random.Int(10, 200),
                    Urls = urls,
                    Condition = (ProductCondition)faker.Random.Int(0, 1),
                    IsActive = true
                });
            }

            return products;
        }
    }

}