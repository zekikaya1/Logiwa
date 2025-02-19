using Logiwa.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logiwa.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.ToTable("product");

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasColumnName("description").HasColumnType("text");
        builder.Property(x => x.StockQuantity).HasColumnName("stock_quantity").IsRequired();
        builder.Property(x => x.CategoryId).HasColumnName("category_id").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        builder.Property(x => x.UpdatedDate).HasColumnName("updated_date").HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.HasOne(x => x.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}