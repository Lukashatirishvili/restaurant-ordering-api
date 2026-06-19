using Microsoft.EntityFrameworkCore;
using RestaurantOrdering.Api.Models;

namespace RestaurantOrdering.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Category>  Categories { get; set; }
    public DbSet<MenuItem>  MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(c => c.Description)
                .HasMaxLength(500);

            entity.HasIndex(c => c.Name).IsUnique();
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(m => m.Description)
                .HasMaxLength(500);

            entity.Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(m => m.Category)
                .WithMany(m => m.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}