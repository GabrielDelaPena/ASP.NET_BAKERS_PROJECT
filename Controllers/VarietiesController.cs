using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bakers.Data;
using Bakers.Models;

namespace Bakers.Controllers
{
    public class VarietiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VarietiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Varieties
        public async Task<IActionResult> Index(string searchField = "")
        {
            List<Variety> varieties = await _context.Variety
                .Where(v => !v.IsHidden)
                .Include(v => v.Products)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchField))
                varieties = varieties.Where(v => v.Name.Contains(searchField) && !v.IsHidden).ToList();

            ViewData["searchField"] = searchField;
            return View(varieties);

        }

        // GET: Varieties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Variety == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variety == null)
            {
                return NotFound();
            }

            return View(variety);
        }

        // GET: Varieties/Create
        public IActionResult Create()
        {
            ViewData["ProductIds"] = new SelectList(_context.Set<Product>(), "Id", "Name");
            return View();
        }

        // POST: Varieties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProductIds")] Variety variety)
        {
            if (ModelState.IsValid)
            {
                List<Product> products = new List<Product>();
                for (int i = 0; i < variety.ProductIds.Count; i++)
                {

                    Product product = await _context.Product.FirstOrDefaultAsync(p => p.Id == variety.ProductIds[i]);
                    products.Add(product);
                }

                variety.Products = products;
                _context.Add(variety);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(variety);
        }

        // GET: Varieties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Variety == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety.FindAsync(id);
            if (variety == null)
            {
                return NotFound();
            }

            ViewData["Products"] = new SelectList(_context.Set<Product>(), "Id", "Name");
            return View(variety);
        }

        // POST: Varieties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Variety variety)
        {
            if (id != variety.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variety);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VarietyExists(variety.Id))
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
            return View(variety);
        }

        // GET: Varieties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Variety == null)
            {
                return NotFound();
            }

            var variety = await _context.Variety
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variety == null)
            {
                return NotFound();
            }

            return View(variety);
        }

        // POST: Varieties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Variety == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Variety'  is null.");
            }
            var variety = await _context.Variety.FindAsync(id);
            variety.IsHidden = true;
            //if (variety != null)
            //{
            //    _context.Variety.Remove(variety);
            //}
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VarietyExists(int id)
        {
          return _context.Variety.Any(e => e.Id == id);
        }
    }
}
