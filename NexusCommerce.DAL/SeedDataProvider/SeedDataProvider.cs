    using Microsoft.AspNetCore.Identity;
using NexusCommerce.DAL.Data.Context;
using NexusCommerce.DAL.Data.Models;

namespace NexusCommerce.DAL.SeedDataProvider
{
    public static class SeedDataProvider
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@nexus.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var customerEmail = "customer@nexus.com";
            var customerUser = await userManager.FindByEmailAsync(customerEmail);
            if (customerUser == null)
            {
                customerUser = new ApplicationUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    FullName = "John Doe Customer",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(customerUser, "Customer123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");

                    if (!context.Carts.Any(c => c.UserId == customerUser.Id))
                    {
                        context.Carts.Add(new Cart
                        {
                            UserId = customerUser.Id,
                            CreatedAt = DateTime.UtcNow
                        });
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.Categories.Any())
            {
                var electronics = new Category { Name = "Electronics", ImageUrl = "/images/electronics.jpg", CreatedAt = DateTime.UtcNow };
                var clothing = new Category { Name = "Clothing", ImageUrl = "/images/clothing.jpg", CreatedAt = DateTime.UtcNow };
                var books = new Category { Name = "Books", ImageUrl = "/images/books.jpg", CreatedAt = DateTime.UtcNow };

                context.Categories.AddRange(electronics, clothing, books);
                await context.SaveChangesAsync();

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product
                        {
                            Name = "Smartphone Pro",
                            Description = "Latest model high-end smartphone with pro camera features.",
                            Price = 999.99m,
                            Stock = 50,
                            ImageUrl = "/images/smartphone.jpg",
                            CategoryId = electronics.Id,
                            CreatedAt = DateTime.UtcNow
                        },
                        new Product
                        {
                            Name = "Noise Cancelling Headphones",
                            Description = "Over-ear active noise cancelling premium headphones.",
                            Price = 249.99m,
                            Stock = 120,
                            ImageUrl = "/images/headphones.jpg",
                            CategoryId = electronics.Id,
                            CreatedAt = DateTime.UtcNow
                        },
                        new Product
                        {
                            Name = "Designer Leather Jacket",
                            Description = "100% genuine black leather slim-fit designer jacket.",
                            Price = 189.50m,
                            Stock = 30,
                            ImageUrl = "/images/jacket.jpg",
                            CategoryId = clothing.Id,
                            CreatedAt = DateTime.UtcNow
                        },
                        new Product
                        {
                            Name = "Software Engineering Handbook",
                            Description = "Complete guide to modern software engineering patterns and practices.",
                            Price = 45.00m,
                            Stock = 200,
                            ImageUrl = "/images/book.jpg",
                            CategoryId = books.Id,
                            CreatedAt = DateTime.UtcNow
                        }
                    );

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
