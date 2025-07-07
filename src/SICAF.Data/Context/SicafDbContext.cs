using Microsoft.EntityFrameworkCore;

using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Context;

public class SicafDbContext(DbContextOptions<SicafDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SicafDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

}