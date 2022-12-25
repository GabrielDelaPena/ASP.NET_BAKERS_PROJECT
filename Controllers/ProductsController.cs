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

namespace Bakers.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BakersDbContext _context;

        public ProductsController(BakersDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchField = "")
        {             
              List<Product> products = await _context.Product
                .Where(p => !p.IsHidden)
                .Include(p => p.Variety)
                .Include(p => p.Orders)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchField))
                products = products.Where(p => p.Name.Contains(searchField)).ToList();

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

        [Authorize(Roles = "admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Variety"] = new SelectList(_context.Set<Variety>(), "Id", "Name");
            ViewData["Orders"] = new SelectList(_context.Set<Order>(), "Id", "Id");
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Image,Favorite,VarietyId,OrderIds")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
    }
}
