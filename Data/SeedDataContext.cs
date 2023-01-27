using Microsoft.EntityFrameworkCore;
using Bakers.Models;
using Bakers.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bakers.Data
{
    public class SeedDataContext
    {
        public static async Task<IActionResult> Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            using (var context = new BakersDbContext(serviceProvider.GetRequiredService<DbContextOptions<BakersDbContext>>()))
            {
                context.Database.Migrate();
                context.Database.EnsureCreated();

                if (!context.Roles.Any())
                {

                    ApplicationUser dummy = new ApplicationUser
                    {
                        Email = "gabriel@snowball.be",
                        EmailConfirmed = true,
                        LockoutEnabled = true,
                        UserName = "Gabriel",
                        FirstName = "Gabriel",
                        LastName = "Dela Peña"
                    };
                    ApplicationUser administrator = new ApplicationUser
                    {
                        Email = "admin@Bakers.be",
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        UserName = "Administrator",
                        FirstName = "Administrator",
                        LastName = "Bakers"
                    };

                    await userManager.CreateAsync(administrator, "Abc!12345");
                    await userManager.CreateAsync(dummy, "Abc!12345");

                    context.Roles.AddRange
                    (
                       new IdentityRole { Id = "admin", Name = "admin", NormalizedName = "ADMIN" },
                       new IdentityRole { Id = "user", Name = "user", NormalizedName = "USER" },
                       new IdentityRole { Id = "employee", Name = "employee", NormalizedName = "EMPLOYEE" }
                    );
                    context.SaveChanges();

                    context.UserRoles.AddRange
                        (
                            new IdentityUserRole<string> { RoleId = "user", UserId = administrator.Id },
                            new IdentityUserRole<string> { RoleId = "admin", UserId = administrator.Id },
                            new IdentityUserRole<string> { RoleId = "employee", UserId = dummy.Id }
                        );
                    context.SaveChanges();

                }


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

                if (!context.Order.Any())
                {
                    context.Order.AddRange
                        (
                            new Order { OrderDate = DateTime.Now, Street = "SomeAddress 12", Zip = "1000", City = "Brussels", IsHidden = false }
                        );
                    context.SaveChanges();
                }

                List<int> orderIds = new List<int>();
                orderIds.Add(1);


                if (!context.Product.Any())
                {
                    context.Product.AddRange
                        (
                            new Product { Name = "Chocolate Cake", Description = "Some Description", Price = 29.99, Image = "https://i.ytimg.com/vi/dsJtgmAhFF4/maxresdefault.jpg", Favorite = false, IsHidden = false, VarietyId = 1, OrderIds = orderIds}
                        );
                    context.SaveChanges();
                }


               

            }
            return null;
        }
    }
}
