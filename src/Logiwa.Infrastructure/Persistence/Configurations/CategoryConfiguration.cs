using Logiwa.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logiwa.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.ToTable("category");

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.MinQuantity).HasColumnName("min_quantity").IsRequired().HasDefaultValue(0);
        builder.Property(x => x.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        builder.Property(x => x.UpdatedDate).HasColumnName("updated_date").HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
    }
}