using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusCommerce.DAL.Data.Models;

namespace NexusCommerce.DAL.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Description).HasMaxLength(1000);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.ImageUrl).HasMaxLength(500);

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
