using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bakers.Models;

namespace Bakers.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Bakers.Models.Product> Product { get; set; }
        public DbSet<Bakers.Models.Variety> Variety { get; set; }
        public DbSet<Bakers.Models.Order> Order { get; set; }
    }
}