using MVCTest.Models;

namespace MVCTest.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Username = "admin", Password = "password", Role = "admin" },
                    new User { Username = "user", Password = "password", Role = "user" }
                );
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Title = "HDD 1TB", Quantity = 55, Price = 74.09M },
                    new Product { Title = "HDD SSD 512GB", Quantity = 102, Price = 190.99M },
                    new Product { Title = "RAM DDR4 16GB", Quantity = 47, Price = 80.32M }
                );
            }

            context.SaveChanges();
        }
    }
}
