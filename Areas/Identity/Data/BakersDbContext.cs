using Bakers.Areas.Identity.Data;
using Bakers.Models;
using Bakers.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bakers.Areas.Identity.Data;

public class BakersDbContext : IdentityDbContext<ApplicationUser>
{
    public BakersDbContext(DbContextOptions<BakersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bakers.Models.Product> Product { get; set; }
    public DbSet<Bakers.Models.Variety> Variety { get; set; }
    public DbSet<Bakers.Models.Order> Order { get; set; }
    public DbSet<Bakers.Models.Language> Language { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
