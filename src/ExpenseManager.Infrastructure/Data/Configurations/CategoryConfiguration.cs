using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Category entity
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table name
        builder.ToTable("Categories");

        // Primary key
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        // Base properties
        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        // Name
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Description
        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(250);

        // UserId (Foreign Key)
        builder.Property(c => c.UserId)
            .IsRequired();

        // Indexes
        builder.HasIndex(c => c.UserId); // Query: get categories by user

        // Composite unique index: same user cannot have duplicate category names
        builder.HasIndex(c => new { c.UserId, c.Name })
            .IsUnique();

        // Relationships
        builder.HasMany<Expense>()
            .WithOne()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Cannot delete category if has expenses
    }
}