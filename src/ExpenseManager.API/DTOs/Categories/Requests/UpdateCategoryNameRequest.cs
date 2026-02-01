namespace ExpenseManager.API.DTOs.Categories.Requests;

/// <summary>
/// Request to update category name
/// </summary>
public record UpdateCategoryNameRequest
{
    /// <summary>
    /// New category name
    /// </summary>
    public required string Name { get; init; }
}