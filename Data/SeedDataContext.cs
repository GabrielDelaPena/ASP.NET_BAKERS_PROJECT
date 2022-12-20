using Microsoft.EntityFrameworkCore;
using Bakers.Models;

namespace Bakers.Data
{
    public class SeedDataContext
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                if (!context.Variety.Any())
                {
                    context.Variety.AddRange
                        (
                            new Variety { Name = "Cake", IsHidden = false },
                            new Variety { Name = "Ice Cream", IsHidden = false },
                            new Variety { Name = "Praline", IsHidden = false }
                        );
                    context.SaveChanges();
                }

                if (!context.Product.Any())
                {
                    context.Product.AddRange
                        (
                            new Product { Name = "Chocolate Cake", Description = "Some Description", Price = 29.99, Image = "Image Test", Favorite = false, IsHidden = false }
                        );
                    context.SaveChanges();
                }

                if (!context.Order.Any())
                {
                    context.Order.AddRange
                        (
                            new Order { OrderDate = DateTime.Now, Street = "SomeAddress 12", Zip = "1000", City = "Brussels", IsHidden = false}
                        );
                    context.SaveChanges();
                }
            }
        }
    }
}
