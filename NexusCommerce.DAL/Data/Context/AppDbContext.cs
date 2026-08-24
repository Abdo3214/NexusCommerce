using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexusCommerce.DAL.Data.Models;

namespace NexusCommerce.DAL.Data.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var auditable = (IAuditable)entityEntry.Entity;
                var now = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    auditable.CreatedAt = now;
                }
                else
                {
                    entityEntry.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                }

                auditable.UpdatedAt = now;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
