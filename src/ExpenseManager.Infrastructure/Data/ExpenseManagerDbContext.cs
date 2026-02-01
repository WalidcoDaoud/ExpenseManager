using Microsoft.EntityFrameworkCore;
using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Infrastructure.Data;

/// <summary>
/// Database context for Expense Manager application
/// </summary>
public class ExpenseManagerDbContext : DbContext
{
    public ExpenseManagerDbContext(DbContextOptions<ExpenseManagerDbContext> options)
        : base(options)
    {
    }

    // DbSets represent tables in the database
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseManagerDbContext).Assembly);
    }
}