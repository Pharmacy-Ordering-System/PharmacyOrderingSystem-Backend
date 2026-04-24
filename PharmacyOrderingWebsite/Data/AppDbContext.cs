using Microsoft.EntityFrameworkCore;
using PharmacyOrderingWebsite.Models;

namespace PharmacyOrderingWebsite.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medicine>()
            .HasOne(m => m.Category)
            .WithMany(c => c.Medicines)
            .HasForeignKey(m => m.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Medicine)
            .WithOne(m => m.Inventory)
            .HasForeignKey<Inventory>(i => i.MedicineId);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<LoyaltyPoints> LoyaltyPoints { get; set; }
    public DbSet<HealthPackage> HealthPackages { get; set; }

}