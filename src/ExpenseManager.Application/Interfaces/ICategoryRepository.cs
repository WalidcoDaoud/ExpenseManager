using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Application.Interfaces;

/// <summary>
/// Repository interface for Category operations
/// </summary>
public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId);
    Task<Category> AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> NameExistsForUserAsync(Guid userId, string name);
}