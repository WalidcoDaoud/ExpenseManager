namespace ExpenseManager.API.DTOs.Categories.Requests;

/// <summary>
/// Request to update category name
/// </summary>
public record UpdateCategoryNameRequest
{
    /// <summary>
    /// New category name
    /// </summary>
    /// <example>Restaurants</example>
    public required string Name { get; init; }
}