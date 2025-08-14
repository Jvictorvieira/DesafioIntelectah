using ConcessionariaAPP.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ConcessionariaAPP.Infrastructure;
public class AppDbContext : IdentityDbContext<Users>
{
    public DbSet<Manufacturers> Manufacturers { get; set; }
    public DbSet<Vehicles> Vehicles { get; set; }
    public DbSet<Clients> Clients { get; set; }
    public DbSet<Sales> Sales { get; set; }
    public DbSet<CarDealership> CarDealerships { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CarDealershipsConfiguration());

        modelBuilder.ApplyConfiguration(new UsersConfiguration());

        modelBuilder.ApplyConfiguration(new ManufacturersConfiguration());

        modelBuilder.ApplyConfiguration(new VehiclesConfiguration());
            
        modelBuilder.ApplyConfiguration(new ClientsConfiguration());

        modelBuilder.ApplyConfiguration(new SalesConfiguration());
        
    }
}
