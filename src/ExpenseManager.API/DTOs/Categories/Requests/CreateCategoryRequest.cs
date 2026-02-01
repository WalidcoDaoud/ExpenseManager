namespace ExpenseManager.API.DTOs.Categories.Requests;

/// <summary>
/// Request to create a new category
/// </summary>
public record CreateCategoryRequest
{
    /// <summary>
    /// Category name
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// User ID who owns the category
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Optional category description
    /// </summary>
    public string? Description { get; init; }
}