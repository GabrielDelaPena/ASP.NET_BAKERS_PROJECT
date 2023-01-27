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
    [Authorize(Roles = "admin")]
    public class OrdersController : Controller
    {
        private readonly BakersDbContext _context;

        public OrdersController(BakersDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
              return View(await _context.Order
                  .Where(o => !o.IsHidden)
                  .Include(o => o.Products)
                  .Include(o => o.User)
                  .ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Products)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["Products"] = new SelectList(_context.Set<Product>(), "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,Street,Zip,City,ProductIds,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Products = new List<Product>();
                foreach (int id in order.ProductIds)
                {
                    order.Products.Add(_context.Product.FirstOrDefault(p => p.Id == id));
                }

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["Products"] = new SelectList(_context.Set<Product>().Where(p => p.Favorite == true), "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,Street,Zip,City,ProductIds,UserId,Delivered")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Order existingOrder = _context.Order.Include(o => o.Products).First(o => o.Id == id);
                    existingOrder.OrderDate = order.OrderDate;
                    existingOrder.Street = order.Street;
                    existingOrder.Zip = order.Zip;
                    existingOrder.City = order.City;
                    existingOrder.IsHidden = order.IsHidden;
                    existingOrder.UserId = order.UserId;
                    existingOrder.Delivered = order.Delivered;
                    existingOrder.Products.Clear();

                    
                    foreach (int productIds in order.ProductIds)
                    {
                        existingOrder.Products.Add(_context.Product.FirstOrDefault(p => p.Id == productIds));
                    }
                    _context.Update(existingOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Products)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            order.IsHidden = true;
            //if (order != null)
            //{
            //    _context.Order.Remove(order);
            //}
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return _context.Order.Any(e => e.Id == id);
        }
    }
}
