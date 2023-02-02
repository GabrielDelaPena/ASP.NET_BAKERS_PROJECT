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
                            new Variety { Name = "Praline", IsHidden = false },
                            new Variety { Name = "Yoghurt", IsHidden = false },
                            new Variety { Name = "Drinks", IsHidden = false },
                            new Variety { Name = "Snacks", IsHidden = false }
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
                                Description = "Indulge in the ultimate chocolate experience with our rich and creamy chocolate cake. This decadent dessert is made with the finest quality cocoa powder, giving it a deep, intense chocolate flavor that will leave you wanting more. The cake is layered with a smooth and velvety chocolate ganache that perfectly complements the fluffy chocolate sponge cake.", 
                                Price = 29.99, 
                                Image = "https://i.ytimg.com/vi/dsJtgmAhFF4/maxresdefault.jpg", 
                                Favorite = true, 
                                IsHidden = false, 
                                VarietyId = 1, 
                                OrderIds = orderIds
                            },
                            new Product 
                            {   Name = "Ice Candy Ube", 
                                Description = "Introducing our delicious and refreshing Ice Candy Ube! This frozen treat is the perfect way to cool down on a hot summer day. Made with creamy ube flavor and a touch of sweetness, our Ice Candy Ube is the perfect balance of flavor and texture. The rich, purple color of the ube is sure to catch your eye and the smooth and creamy texture will have you coming back for more.\r\n\r\nThis treat is perfect for those who love ube flavor, and is sure to be a hit with kids and adults alike.", 
                                Price = 9.99, 
                                Image = "https://simplybakings.com/wp-content/uploads/2020/07/ube-ice-candy-2-300x200.jpg", 
                                Favorite = true, 
                                IsHidden = false, 
                                VarietyId = 2
                            },
                            new Product
                            {
                                Name = "Banana cake",
                                Description = "Our Banana Cake is a classic and delicious treat that will make your taste buds dance with joy. Made with ripe, sweet bananas and the finest ingredients, this cake is soft, moist, and full of flavor. The light and fluffy sponge is infused with the natural sweetness of bananas, making every bite a mouth-watering experience. The cake is covered in a rich and creamy cream cheese frosting that adds an extra layer of richness and a tangy note to the overall taste.",
                                Price = 29.99,
                                Image = "https://food-images.files.bbci.co.uk/food/recipes/easiest_ever_banana_cake_42108_16x9.jpg",
                                Favorite = true,
                                IsHidden = false,
                                VarietyId = 1
                            },
                            new Product
                            {
                                Name = "Lemon yoghurt cake",
                                Description = "ts bright and fresh flavor. This cake is made with a moist and fluffy sponge that is infused with the zesty flavor of fresh lemons. It is then layered with a creamy lemon yoghurt frosting that provides a perfect balance between sweetness and tartness. The cake is finished with a sprinkle of lemon zest on top, giving it a fresh and fruity aroma.",
                                Price = 49.99,
                                Image = "https://img.taste.com.au/BoBlm2aC/taste/2016/11/lemon-yoghurt-cake-with-syrup-62916-1.jpeg",
                                Favorite = true,
                                IsHidden = false,
                                VarietyId = 1
                            },
                            new Product
                            {
                                Name = "Leonidas",
                                Description = "Leonidas is a luxurious Belgian chocolate brand that has been crafting fine chocolates since 1913. Known for its high-quality ingredients and traditional Belgian chocolate-making techniques, Leonidas offers a range of chocolates that are both rich in flavor and beautiful in presentation. From classic dark chocolates to fruit-filled truffles and creamy pralines, Leonidas has something for every chocolate lover. Each chocolate is crafted with care and precision, ensuring a perfect balance of flavors and textures.",
                                Price = 12.99,
                                Image = "https://kadotheek.be/wp-content/uploads/leonidas-doos-pralines-1kg.jpg",
                                Favorite = true,
                                IsHidden = false,
                                VarietyId = 3
                            },
                            new Product
                            {
                                Name = "Ardelis",
                                Description = "Ardelis is a luxury bakery specializing in the art of baking the most delicious and visually stunning desserts. Our bakery is dedicated to creating the finest and most delectable desserts using only the freshest and finest ingredients. Each dessert is crafted with the utmost care, precision, and attention to detail, making every bite an unforgettable experience.",
                                Price = 12.99,
                                Image = "https://www.ardelis.be/wp-content/uploads/2019/11/verpakte_chocolades_image.php-10.png",
                                Favorite = true,
                                IsHidden = false,
                                VarietyId = 3
                            },
                            new Product
                            {
                                Name = "Flourless orange cake",
                                Description = "Introducing our Flourless Orange Cake, a delicious and gluten-free dessert that is perfect for any special occasion. This cake is made with almond flour and juicy oranges, which creates a light and fluffy texture that will melt in your mouth. The orange flavor is subtle and not overpowering, allowing the sweetness of the cake to shine through.",
                                Price = 69.99,
                                Image = "https://www.recipetineats.com/wp-content/uploads/2020/10/Flourless-Orange-Cake_5-SQ.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 1
                            },
                            new Product
                            {
                                Name = "Ice candy chocolate",
                                Description = "Indulge in the rich and creamy delight of our Ice Candy Chocolate! Each bite is a symphony of flavors, with a smooth and velvety texture that will melt in your mouth. Our Ice Candy Chocolate is made with premium quality chocolate and cream, blended to perfection to create a heavenly treat. The chocolate is perfectly balanced, with a rich and intense flavor that will satisfy your sweet tooth cravings. The creamy texture will leave you feeling refreshed and satisfied, while the rich flavor will linger on your taste buds long after you finish. Whether you're looking for a sweet snack, a dessert after a meal, or just want to treat yourself, our Ice Candy Chocolate is the perfect choice. So, grab one today and experience the ultimate chocolate indulgence!",
                                Price = 5.99,
                                Image = "http://images.summitmedia-digital.com/yummyph/images/2019/05/24/miloicecandyrecipe.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 2
                            },
                            new Product
                            {
                                Name = "Mochi ice cream",
                                Description = "Mochi ice cream is a delightful dessert that is made from chewy and sweet mochi (rice cake) wrapped around a creamy and smooth ice cream filling. This dessert is the perfect blend of texture and flavor that will tantalize your taste buds. The mochi dough is soft and chewy, while the ice cream filling is smooth and rich, providing the perfect balance of sweetness. Mochi ice cream is available in a variety of flavors, from classic vanilla to more exotic flavors such as green tea and black sesame. Whether you're a fan of fruity or creamy flavors, there's a mochi ice cream for everyone! Mochi ice cream is a perfect treat for hot summer days, or for when you're in the mood for something sweet and indulgent. ",
                                Price = 5.99,
                                Image = "https://www.justonecookbook.com/wp-content/uploads/2020/08/Mochi-Ice-Cream-8680-I-500x375.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 2
                            },
                            new Product
                            {
                                Name = "De zoete oogst",
                                Description = "De zoete oogst is a luxury bakery specializing in the art of baking the most delicious and visually stunning desserts. Our bakery is dedicated to creating the finest and most delectable desserts using only the freshest and finest ingredients. Each dessert is crafted with the utmost care, precision, and attention to detail, making every bite an unforgettable experience.",
                                Price = 15.99,
                                Image = "https://usercontent.one/wp/www.dezoeteoogst.be/wp-content/uploads/2021/02/pralinesalgemeen-scaled.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 3
                            },
                            new Product
                            {
                                Name = "Sherbet",
                                Description = "Sherbet is a sweet and fruity dessert that's perfect for those hot summer days. Made with fruit juice or puree, sugar and sometimes dairy, this treat is both refreshing and satisfying. The delicate balance of sweet and tart flavors creates a unique and satisfying experience with each bite. Sherbet comes in a wide range of flavors, from classic favorites like lemon and orange, to more unique options like raspberry and mango. Whether you enjoy it on its own, or as a topping for ice cream, sherbet is the perfect choice for a sweet and fruity dessert. So why not cool off with a scoop of this delicious treat today!",
                                Price = 9.99,
                                Image = "https://www.thespruceeats.com/thmb/O2QomtonGe207BJOxbNXec-Xd7w=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/rainbow-sherbet-2500-57638b923df78c98dcd30fa9.jpg",
                                Favorite = false,
                                IsHidden = false,
                                VarietyId = 2
                            }
                        );
                    context.SaveChanges();
                }

                if (!context.Order.Any())
                {
                    context.Order.AddRange
                        (
                            new Order { OrderDate = DateTime.Now, Street = "SomeAddress 12", Zip = "1000", City = "Brussels", IsHidden = false },
                            new Order { OrderDate = DateTime.Now, Street = "Newstreet 15", Zip = "1000", City = "Anderlecht", IsHidden = false }
                        );
                    context.SaveChanges();
                }

                if (!context.Language.Any())
                {
                    // Initialize the languages
                    context.Language.AddRange
                        (
                            new Language() { Id = "-", Name = "-", Cultures = "", IsShown = false },
                            new Language() { Id = "en", Name = "English", Cultures = "UK;US", IsShown = true },
                            new Language() { Id = "fr", Name = "français", Cultures = "BE;FR", IsShown = true },
                            new Language() { Id = "nl", Name = "Nederlands", Cultures = "BE;NL", IsShown = true }
                        );
                    context.SaveChanges();
                }

                Language.Initialize(context);


            }
            return null;
        }
    }
}
