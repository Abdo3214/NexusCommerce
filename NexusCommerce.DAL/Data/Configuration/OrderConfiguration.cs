using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCommerce.DAL.Data.Models;

namespace NexusCommerce.DAL.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.UserId).IsRequired();
            builder.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
            builder.Property(o => o.Status).IsRequired().HasMaxLength(50);

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
