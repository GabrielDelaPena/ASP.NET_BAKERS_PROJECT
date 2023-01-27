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
                            new Product 
                            {   Name = "Chocolate Cake", 
                                Description = "Some Description", 
                                Price = 29.99, 
                                Image = "https://i.ytimg.com/vi/dsJtgmAhFF4/maxresdefault.jpg", 
                                Favorite = true, 
                                IsHidden = false, 
                                VarietyId = 1, 
                                OrderIds = orderIds
                            },
                            new Product 
                            {   Name = "Ice Candy Ube", 
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.", 
                                Price = 9.99, 
                                Image = "https://simplybakings.com/wp-content/uploads/2020/07/ube-ice-candy-2-300x200.jpg", 
                                Favorite = true, 
                                IsHidden = false, 
                                VarietyId = 2
                            },
                            new Product
                            {
                                Name = "Banana cake",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 29.99,
                                Image = "https://food-images.files.bbci.co.uk/food/recipes/easiest_ever_banana_cake_42108_16x9.jpg",
                                Favorite = true,
                                IsHidden = false,
                                VarietyId = 1
                            },
                            new Product
                            {
                                Name = "Lemon yoghurt cake",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 49.99,
                                Image = "https://img.taste.com.au/BoBlm2aC/taste/2016/11/lemon-yoghurt-cake-with-syrup-62916-1.jpeg",
                                Favorite = true,
                                IsHidden = false,
                                VarietyId = 1
                            },
                            new Product
                            {
                                Name = "Leonidas",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 12.99,
                                Image = "https://kadotheek.be/wp-content/uploads/leonidas-doos-pralines-1kg.jpg",
                                Favorite = true,
                                IsHidden = false,
                                VarietyId = 3
                            },
                            new Product
                            {
                                Name = "Ardelis",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 12.99,
                                Image = "https://www.ardelis.be/wp-content/uploads/2019/11/verpakte_chocolades_image.php-10.png",
                                Favorite = true,
                                IsHidden = false,
                                VarietyId = 3
                            },
                            new Product
                            {
                                Name = "Flourless orange cake",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 69.99,
                                Image = "https://www.recipetineats.com/wp-content/uploads/2020/10/Flourless-Orange-Cake_5-SQ.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 1
                            },
                            new Product
                            {
                                Name = "Ice candy chocolate",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 5.99,
                                Image = "http://images.summitmedia-digital.com/yummyph/images/2019/05/24/miloicecandyrecipe.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 2
                            },
                            new Product
                            {
                                Name = "Mochi ice cream",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 5.99,
                                Image = "https://www.justonecookbook.com/wp-content/uploads/2020/08/Mochi-Ice-Cream-8680-I-500x375.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 2
                            },
                            new Product
                            {
                                Name = "De zoete oogst",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 15.99,
                                Image = "https://usercontent.one/wp/www.dezoeteoogst.be/wp-content/uploads/2021/02/pralinesalgemeen-scaled.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 3
                            },
                            new Product
                            {
                                Name = "Sherbet",
                                Description = "Lorem Ipsum is slechts een proeftekst uit het drukkerij- en zetterijwezen. Lorem Ipsum is de standaard proeftekst in deze bedrijfstak sinds de 16e eeuw, toen een onbekende drukker een zethaak met letters nam en ze door elkaar husselde om een font-catalogus te maken. Het heeft niet alleen vijf eeuwen overleefd maar is ook, vrijwel onveranderd, overgenomen in elektronische letterzetting. Het is in de jaren '60 populair geworden met de introductie van Letraset vellen met Lorem Ipsum passages en meer recentelijk door desktop publishing.",
                                Price = 9.99,
                                Image = "https://www.thespruceeats.com/thmb/O2QomtonGe207BJOxbNXec-Xd7w=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/rainbow-sherbet-2500-57638b923df78c98dcd30fa9.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 2
                            }
                        );
                    context.SaveChanges();
                }


               

            }
            return null;
        }
    }
}
