using Bakers.Areas.Identity.Data;
using Bakers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace Bakers.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class PaymentController : Controller
    {
        private readonly BakersDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentController(BakersDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder([Bind("Id,Street,Zip,City")] Order order)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

                string cartString = HttpContext.Session.GetString("Cart");
                Cart cart = JsonConvert.DeserializeObject<Cart>(cartString);

                order.Products = new List<Product>();
                foreach (var item in cart.Items)
                {
                    order.Products.Add(_context.Product.FirstOrDefault(p => p.Id == item.Id));
                }
                order.UserId = user.Id;

                _context.Add(order);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Purchased", "Payment");
            }
            return View(order);
        }

        public IActionResult Purchased()
        {
            return View();
        }
    }
}
