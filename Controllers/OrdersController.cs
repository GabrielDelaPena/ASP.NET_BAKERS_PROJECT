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
using Bakers.Models.ViewModels;
using System.Security.Claims;

namespace Bakers.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BakersDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(BakersDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "admin")]
        // GET: Orders
        public async Task<IActionResult> Index(string searchField = " ")
        {
            return View(await _context.Order
                  .Where(o => !o.IsHidden)
                  .Include(o => o.Products)
                  .Include(o => o.User)
                  .ToListAsync());
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["Products"] = new SelectList(_context.Set<Product>(), "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        [Authorize(Roles = "admin")]
        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,Street,Zip,City,ProductIds,UserId")] Order order)
        {
            if (order.ProductIds == null)
            {
                order.ProductIds = new List<int>();
            }

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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

            if (order.ProductIds == null)
            {
                order.ProductIds = new List<int>();
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin, user, employee")]
        public async Task<IActionResult> MyOrders()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var userIdClaim = currentUser.FindFirst(ClaimTypes.NameIdentifier);
            string userId = "";

            if (userIdClaim != null)
            {
                userId = userIdClaim.Value;
            }

            var orders = await _context.Order
                .Where(o => !o.IsHidden && o.UserId == userId)
                .Include(o => o.Products)
                .Include(o => o.User)
                .ToListAsync();

            if (orders.Count > 0)
            {
                return View(await _context.Order
                    .Where(o => !o.IsHidden && o.UserId == userId)
                    .Include(o => o.Products)
                    .Include(o => o.User)
                    .ToListAsync());
            } else
            {
                ViewData["NoOrders"] = "Client has no orders.";
                return View();
            }
        }
    }
}
