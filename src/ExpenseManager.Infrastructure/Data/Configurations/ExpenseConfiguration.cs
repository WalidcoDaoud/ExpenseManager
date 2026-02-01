using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Expense entity
/// </summary>
public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        // Table name
        builder.ToTable("Expenses");

        // Primary key
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        // Base properties
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        // Description
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(200);

        // Amount (Value Object - Money)
        builder.OwnsOne(e => e.Amount, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)") // 18 total digits, 2 after decimal point
                .HasColumnName("Amount");

            money.Property(m => m.Currency)
                .IsRequired()
                .HasMaxLength(3) // ISO 4217: BRL, USD, EUR
                .HasColumnName("Currency")
                .HasDefaultValue("BRL");
        });

        // Date
        builder.Property(e => e.Date)
            .IsRequired()
            .HasColumnType("datetime2"); // More precise than datetime

        // Type (Enum)
        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<int>(); // Store as integer in database

        // PaymentMethod (Enum nullable)
        builder.Property(e => e.PaymentMethod)
            .IsRequired(false)
            .HasConversion<int>();

        // Notes
        builder.Property(e => e.Notes)
            .IsRequired(false)
            .HasMaxLength(500);

        // Foreign Keys
        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.CategoryId)
            .IsRequired();

        // Indexes for common queries
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.CategoryId);
        builder.HasIndex(e => e.Date); // Query by date range
        builder.HasIndex(e => new { e.UserId, e.Date }); // User expenses in date range
        builder.HasIndex(e => e.Type); // Filter by Expense/Income
    }
}