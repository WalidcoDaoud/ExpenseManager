using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for User entity
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name
        builder.ToTable("Users");

        // Primary key
        builder.HasKey(u => u.Id);

        // Properties from Entity base class
        builder.Property(u => u.Id)
            .ValueGeneratedNever(); // Guid is generated in the application, not by database

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired(false);

        // Name
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Email (Value Object)
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(320) // Max email length (RFC 5321)
                .HasColumnName("Email");

            // Unique index on email
            email.HasIndex(e => e.Value)
                .IsUnique();
        });

        // Password (Value Object)
        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(p => p.Hash)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("PasswordHash");

            password.Property(p => p.Salt)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("PasswordSalt");
        });

        // IsActive
        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // LastLoginAt
        builder.Property(u => u.LastLoginAt)
            .IsRequired(false);

        // Relationships
        builder.HasMany<Category>()
            .WithOne()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Cannot delete user if has categories

        builder.HasMany<Expense>()
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Cannot delete user if has expenses
    }
}