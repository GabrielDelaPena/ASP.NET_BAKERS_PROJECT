using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bakers.Areas.Identity.Data;
using Bakers.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Bakers.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly BakersDbContext _context;

        public UsersController(BakersDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Users
                .Include(u => u.Orders);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName")] ApplicationUser applicationUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == applicationUser.Id);
            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == applicationUser.Id);

            if (ModelState.IsValid)
            {
                user.FirstName = applicationUser.FirstName;
                user.LastName = applicationUser.LastName;
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }
    }
}
