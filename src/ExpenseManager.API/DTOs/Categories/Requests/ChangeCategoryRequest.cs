namespace ExpenseManager.API.DTOs.Categories.Requests;

/// <summary>
/// Request to change category 
/// </summary>

public record ChangeCategoryRequest
{
    /// <summary>
    /// New category name
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// New category description
    /// </summary>
    public string? Description { get; init; }
}
