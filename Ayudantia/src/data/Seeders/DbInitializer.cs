using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data.Seeders;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Mappers;
using Ayudantia.Src.Models;

using Bogus;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ayudantia.Src.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>()
                ?? throw new InvalidOperationException("Could not get UserManager");

            var context = scope.ServiceProvider.GetRequiredService<StoreContext>()
                ?? throw new InvalidOperationException("Could not get StoreContext");

            await SeedData(context, userManager);
        }

        private static async Task SeedData(StoreContext context, UserManager<User> userManager)
        {
            await context.Database.MigrateAsync();

            if (!context.Products.Any())
            {
                var products = ProductSeeder.GenerateProducts(10);
                context.Products.AddRange(products);
            }

            if (!context.Users.Any())
            {
                var userDtos = UserSeeder.GenerateUserDtos(10);
                await UserSeeder.CreateUsers(userManager, userDtos);
            }

            await context.SaveChangesAsync();
        }
}