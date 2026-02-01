using ExpenseManager.API.DTOs.Expenses.Requests;
using ExpenseManager.Application.Interfaces;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ExpensesController(
        IExpenseRepository expenseRepository,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository)
    {
        _expenseRepository = expenseRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// Creates a new expense
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request)
    {
        try
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
                return NotFound(new { error = "User not found" });

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                return NotFound(new { error = "Category not found" });

            if (category.UserId != request.UserId)
                return BadRequest(new { error = "Category does not belong to user" });

            var amount = new Money(request.Amount, request.Currency);
            var expense = new Expense(
                    description: request.Description,
                    amount: amount,
                    date: request.Date,
                    userId: request.UserId,
                    categoryId: request.CategoryId,
                    type: request.Type,
                    paymentMethod: request.PaymentMethod,
                    notes: request.Notes
                    );

            await _expenseRepository.AddAsync(expense);

            return CreatedAtAction(
                nameof(GetExpenseById), // ← Vai criar depois
                new { id = expense.Id },
                new
                {
                    id = expense.Id,
                    description = expense.Description,
                    amount = expense.Amount.Amount,
                    currency = expense.Amount.Currency,
                    date = expense.Date,
                    type = expense.Type.ToString(),
                    paymentMethod = expense.PaymentMethod?.ToString(),
                    notes = expense.Notes,
                    userId = expense.UserId,
                    categoryId = expense.CategoryId,
                    createdAt = expense.CreatedAt
                });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Gets all expenses
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllExpenses()
    {
        var expenses = await _expenseRepository.GetAllAsync();
        var response = expenses.Select(expense => new
        {
            id = expense.Id,
            description = expense.Description,
            amount = expense.Amount.Amount,
            currency = expense.Amount.Currency,
            date = expense.Date,
            type = expense.Type.ToString(),
            paymentMethod = expense.PaymentMethod?.ToString(),
            notes = expense.Notes,
            userId = expense.UserId,
            categoryId = expense.CategoryId,
            createdAt = expense.CreatedAt
        });
        return Ok(response);
    }

    /// <summary>
    /// Gets an expense by ID (placeholder for CreatedAtAction)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseById(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);

        if (expense == null)
            return NotFound(new { error = "Expense not found" });

        return Ok(new
        {
            id = expense.Id,
            description = expense.Description,
            amount = expense.Amount.Amount,
            currency = expense.Amount.Currency,
            date = expense.Date,
            type = expense.Type.ToString(),
            paymentMethod = expense.PaymentMethod?.ToString(),
            notes = expense.Notes,
            userId = expense.UserId,
            categoryId = expense.CategoryId,
            createdAt = expense.CreatedAt,
            updatedAt = expense.UpdatedAt
        });
    }

    /// <summary>
    /// Gets expenses by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpensesByUserId(Guid userId)
    {
        var expenses = await _expenseRepository.GetByUserIdAsync(userId);
        var response = expenses.Select(expense => new
        {
            id = expense.Id,
            description = expense.Description,
            amount = expense.Amount.Amount,
            currency = expense.Amount.Currency,
            date = expense.Date,
            type = expense.Type.ToString(),
            paymentMethod = expense.PaymentMethod?.ToString(),
            notes = expense.Notes,
            userId = expense.UserId,
            categoryId = expense.CategoryId,
            createdAt = expense.CreatedAt
        });
        return Ok(response);
    }

    /// <summary>
    /// Gets categories by category id
    /// </summary>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpensesByCategoryId(Guid categoryId)
    {
        var expenses = await _expenseRepository.GetByCategoryIdAsync(categoryId);
        var response = expenses.Select(expense => new
        {
            id = expense.Id,
            description = expense.Description,
            amount = expense.Amount.Amount,
            currency = expense.Amount.Currency,
            date = expense.Date,
            type = expense.Type.ToString(),
            paymentMethod = expense.PaymentMethod?.ToString(),
            notes = expense.Notes,
            userId = expense.UserId,
            categoryId = expense.CategoryId,
            createdAt = expense.CreatedAt
        });
        return Ok(response);
    }

    /// <summary>
    /// 
    /// </summary>
}
