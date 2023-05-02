using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P013EStore.Core.Entities;

namespace P013EStore.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Image).HasMaxLength(100);
            builder.Property(x => x.ProductCode).HasMaxLength(50);
            // FluentAPI ile class lar arası ilişki kurma
            builder.HasOne(x=>x.Brand).WithMany(x=>x.Products).HasForeignKey(f=>f.BrandId); // Brand class ı ile 1 e çok ilişki kurduk.
            builder.HasOne(x=>x.Category).WithMany(x=>x.Products).HasForeignKey(x=>x.CategoryId); // Category class ı ile 1 e çok ilişki kurduk.
        }
    }
}
