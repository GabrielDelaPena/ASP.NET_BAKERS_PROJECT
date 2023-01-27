using Bakers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace Bakers.Controllers
{
    public class CartController : Controller
    {

        public IActionResult Index()
        {
            string cartString = HttpContext.Session.GetString("Cart");
            

            if (cartString != null) {
                Cart cart = JsonConvert.DeserializeObject<Cart>(cartString);
                return View(cart);
            }

            return View();
        }

        public IActionResult Payment()
        {
            return RedirectToAction("Index", "Payment");
        }

        public IActionResult RemoveItem(int Id)
        {
            string cartString = HttpContext.Session.GetString("Cart");
            Cart cart = JsonConvert.DeserializeObject<Cart>(cartString);

            List<Product> items = new List<Product>();

            foreach (Product product in cart.Items)
            {
                if (product.Id != Id)
                {
                    items.Add(product);
                }
            }

            cart.Items = items;

            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

            return RedirectToAction("Index", "Cart");
        }
    }
}
