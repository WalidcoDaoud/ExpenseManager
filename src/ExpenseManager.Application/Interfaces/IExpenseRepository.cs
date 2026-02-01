using System;
using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Application.Interfaces;

/// <summary>
/// Repository interface for Expense operations
/// </summary>
public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(Guid id);
    Task<IEnumerable<Expense>> GetAllAsync();
    Task<IEnumerable<Expense>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Expense>> GetByCategoryIdAsync(Guid categoryId);
    Task<IEnumerable<Expense>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
    Task<Expense> AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
