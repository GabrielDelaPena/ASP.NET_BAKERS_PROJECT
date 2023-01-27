using Bakers.Areas.Identity.Data;
using Bakers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace Bakers.Controllers
{
    
    public class SuggestionsController : Controller
    {
        private readonly BakersDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public SuggestionsController(BakersDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7009/api/ProductsApi");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Products");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            return View(products);
        }

        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7009/api/ProductsApi/" + id.ToString());

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Products");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            Product product = JsonConvert.DeserializeObject<Product>(jsonData);
            return View(product);
        }

        [Authorize(Roles = "admin, employee")]
        public IActionResult Create()
        {
            ViewData["Variety"] = new SelectList(_context.Set<Variety>(), "Id", "Name");
            ViewData["Orders"] = new SelectList(_context.Set<Order>(), "Id", "Id");
            return View();
        }

        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            product.Favorite = false;
            var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:7009/api/ProductsApi/", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Suggestions");
            }

            return BadRequest();
        }

        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["Variety"] = new SelectList(_context.Set<Variety>(), "Id", "Name", product.VarietyId);
            ViewData["Orders"] = new SelectList(_context.Set<Order>(), "Id", "Id");
            return View(product);
        }

        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image,Favorite,VarietyId")] Product product)
        {
            var client = new HttpClient();
            var response = await client.PutAsync($"https://localhost:7009/api/ProductsApi/{id}", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Suggestions");
            }

            return BadRequest();

            ViewData["VarietyId"] = new SelectList(_context.Set<Variety>(), "Id", "Name", product.VarietyId);
            return View(product);
        }

        [Authorize(Roles = "admin")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Variety)
                .Include(p => p.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var client = new HttpClient();
            var response = await client.DeleteAsync($"https://localhost:7009/api/ProductsApi/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Suggestions");
            }

            return BadRequest();
            return RedirectToAction("Index", "Products");
        }

    }
}
