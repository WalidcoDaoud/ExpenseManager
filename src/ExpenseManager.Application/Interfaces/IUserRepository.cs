using ExpenseManager.Domain.Entities;

namespace ExpenseManager.Application.Interfaces;

/// <summary>
/// Repository interface for User operations
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
}