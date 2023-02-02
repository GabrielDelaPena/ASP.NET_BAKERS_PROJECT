using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bakers.Data;
using Bakers.Models;
using Bakers.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Session;
using static System.Collections.Specialized.BitVector32;

namespace Bakers.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BakersDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public ProductsController(BakersDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchField = "")
        {             
              List<Product> products = await _context.Product
                .Where(p => !p.IsHidden && p.Favorite)
                .Include(p => p.Variety)
                .Include(p => p.Orders)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchField))
                products = products.Where(p => p.Name.Contains(searchField)).ToList();

            if (!products.Any())
            {
                ViewData["NoProducts"] = "No products found.";
            }

            ViewData["searchField"] = searchField;
            return View(products);
        }

        // GET: Products/Details/5  
        public async Task<IActionResult> Details(int? id)
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

        [Authorize(Roles = "admin, employee")]
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Variety"] = new SelectList(_context.Set<Variety>(), "Id", "Name");
            ViewData["Orders"] = new SelectList(_context.Set<Order>(), "Id", "Id");
            return View();
        }

        [Authorize(Roles = "admin, employee")]
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Image,VarietyId,OrderIds")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Favorite = true;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "admin, employee")]
        // GET: Products/Edit/5
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
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image,Favorite,VarietyId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VarietyId"] = new SelectList(_context.Set<Variety>(), "Id", "Name", product.VarietyId);
            return View(product);
        }

        [Authorize(Roles = "admin, employee")]
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

        [Authorize(Roles = "admin, employee")]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            product.IsHidden = true;
            //if (product != null)
            //{
            //    _context.Product.Remove(product);
            //}
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Product.Any(e => e.Id == id);
        }

        [Authorize(Roles = "admin, user, employee")]
        public async Task<IActionResult> AddToCart(string name = "")
        {
            Product product = await _context.Product.FirstOrDefaultAsync(p => p.Name.Equals(name));
            string cartString = HttpContext.Session.GetString("Cart");
           

            if (cartString == null)
            {
                List<Product> items = new List<Product>();
                items.Add(product);

                Cart cart = new Cart();
                cart.Items = items;

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            } else
            {
                Cart cart = JsonConvert.DeserializeObject<Cart>(cartString);
                cart.Items.Add(product);

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("Index", "Cart");
        }

    }
}
