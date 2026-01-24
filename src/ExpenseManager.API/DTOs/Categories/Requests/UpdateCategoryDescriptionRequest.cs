namespace ExpenseManager.API.DTOs.Categories.Requests;

/// <summary>
/// Request to update category description
/// </summary>
public record UpdateCategoryDescriptionRequest
{
    /// <summary>
    /// New category description
    /// </summary>
    /// <example>Expenses for dining out</example>
    public string? Description { get; init; }
}