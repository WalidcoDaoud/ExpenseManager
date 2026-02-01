namespace ExpenseManager.API.DTOs.Expenses.Requests;

/// <summary>
/// Request to update expense description
/// </summary>

public record UpdateExpenseDescriptionRequest
{
    /// <summary>
    /// New expense description
    /// </summary>
    public required string Description { get; init; }
}
