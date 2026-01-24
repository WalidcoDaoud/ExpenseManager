namespace ExpenseManager.API.DTOs.Categories.Requests;

/// <summary>
/// Request to create a new category
/// </summary>
public record CreateCategoryRequest
{
    /// <summary>
    /// Category name
    /// </summary>
    /// <example>Food</example>
    public required string Name { get; init; }

    /// <summary>
    /// User ID who owns the category
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Optional category description
    /// </summary>
    /// <example>Expenses related to restaurants and groceries</example>
    public string? Description { get; init; }
}