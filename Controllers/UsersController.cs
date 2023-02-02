using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bakers.Areas.Identity.Data;
using Bakers.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Bakers.Models.ViewModels;
using System.Collections.Generic;

namespace Bakers.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly BakersDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(BakersDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //var applicationContext = _context.Users
            //    .Include(u => u.Orders);
            //return View(await applicationContext.ToListAsync());
            List<UserViewModel> vmUsers = new List<UserViewModel>();
            List<ApplicationUser> users = _context.Users.ToList();
            foreach (ApplicationUser user in users)
            {
                vmUsers.Add(new UserViewModel 
                { 
                    UserName = user.UserName, 
                    Email = user.Email, 
                    FirstName = user.FirstName,
                    LastName = user.LastName,   
                    Roles = (from userRole in _context.UserRoles
                             where userRole.UserId == user.Id
                             orderby userRole.RoleId
                             select userRole.RoleId).ToList()
                });
            }
            return View(vmUsers);
        }

        public IActionResult Edit(string userName)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            RoleViewModel rvm = new RoleViewModel()
            {
                UserName = user.UserName,
                Roles = (from userRole in _context.UserRoles
                         where userRole.UserId == user.Id
                         orderby userRole.RoleId
                         select userRole.RoleId).ToList()
            };
            ViewData["RoleIds"] = new MultiSelectList(_context.Roles.OrderBy(c => c.Name), "Id", "Name", rvm.Roles);
            return View(rvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("UserName,Roles")] RoleViewModel _model)
        {
            //var user = _context.Users.FirstOrDefault(u => u.Id == applicationUser.Id);
            //var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == applicationUser.Id);

            //if (ModelState.IsValid)
            //{
            //    user.FirstName = applicationUser.FirstName;
            //    user.LastName = applicationUser.LastName;
            //    _context.Users.Update(user);
            //    _context.SaveChanges();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(applicationUser);
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == _model.UserName); // User
            List<IdentityUserRole<string>> roles = _context.UserRoles.Where(ur => ur.UserId == user.Id).ToList(); // Roles
            foreach (IdentityUserRole<string> role in roles) // Clear user roles
            {
                _context.Remove(role);
            }
            if (_model.Roles != null)
            {
                foreach (string roleId in _model.Roles)
                {
                    _context.UserRoles.Add(new IdentityUserRole<string>() { RoleId = roleId, UserId = user.Id });
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string? userName)
        {
            ApplicationUser rvmuser = _context.Users.FirstOrDefault(u => u.UserName == userName);
            RoleViewModel rvm = new RoleViewModel()
            {
                UserName = rvmuser.UserName,
                Roles = (from userRole in _context.UserRoles
                         where userRole.UserId == rvmuser.Id
                         orderby userRole.RoleId
                         select userRole.RoleId).ToList()
            };

            if (userName == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(p => p.Orders)
                .FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return NotFound();
            }

            string result = string.Join(", ", rvm.Roles);
            ViewData["Roles"] = result;

            return View(user);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Delete(string? userName)
        {
            ApplicationUser rvmuser = _context.Users.FirstOrDefault(u => u.UserName == userName);
            RoleViewModel rvm = new RoleViewModel()
            {
                UserName = rvmuser.UserName,
                Roles = (from userRole in _context.UserRoles
                         where userRole.UserId == rvmuser.Id
                         orderby userRole.RoleId
                         select userRole.RoleId).ToList()
            };

            if (userName == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(p => p.Orders)
                .FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return NotFound();
            }

            string result = string.Join(", ", rvm.Roles);
            ViewData["Roles"] = result;

            return View(user);
        }

        public async Task<IActionResult> ConfirmedDelete(string? userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest();
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index", "Users");
        }
    }
}
