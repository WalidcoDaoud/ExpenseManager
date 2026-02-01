using Microsoft.EntityFrameworkCore;
using ExpenseManager.Application.Interfaces;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Infrastructure.Data;

namespace ExpenseManager.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ExpenseManagerDbContext _context;

    public CategoryRepository(ExpenseManagerDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<Category> AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await GetByIdAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> NameExistsForUserAsync(Guid userId, string name)
    {
        return await _context.Categories
            .AnyAsync(c => c.UserId == userId && c.Name == name);
    }
}