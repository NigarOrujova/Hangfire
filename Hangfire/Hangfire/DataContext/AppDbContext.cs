using Hangfire.Models;
using Microsoft.EntityFrameworkCore;

namespace Hangfire.DataContext;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions options):base(options)
    {
        
    }

    public DbSet<Info> Infos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
