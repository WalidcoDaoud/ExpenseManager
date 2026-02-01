using Microsoft.EntityFrameworkCore;
using ExpenseManager.Application.Interfaces;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Infrastructure.Data;

namespace ExpenseManager.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseManagerDbContext _context;

    public ExpenseRepository(ExpenseManagerDbContext context)
    {
        _context = context;
    }

    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        return await _context.Expenses.FindAsync(id);
    }

    public async Task<IEnumerable<Expense>> GetAllAsync()
    {
        return await _context.Expenses
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Expenses
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Expenses
            .Where(e => e.CategoryId == categoryId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByUserAndDateRangeAsync(
        Guid userId,
        DateTime startDate,
        DateTime endDate)
    {
        return await _context.Expenses
            .Where(e => e.UserId == userId && e.Date >= startDate && e.Date <= endDate)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<Expense> AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task UpdateAsync(Expense expense)
    {
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var expense = await GetByIdAsync(id);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Expenses.AnyAsync(e => e.Id == id);
    }
}