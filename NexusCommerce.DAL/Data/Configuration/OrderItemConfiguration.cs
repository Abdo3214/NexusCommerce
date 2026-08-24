using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCommerce.DAL.Data.Models;

namespace NexusCommerce.DAL.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");

            builder.HasOne(oi => oi.Product)
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
