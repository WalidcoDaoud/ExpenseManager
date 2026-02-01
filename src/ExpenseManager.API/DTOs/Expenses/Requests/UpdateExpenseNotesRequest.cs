namespace ExpenseManager.API.DTOs.Expenses.Requests;

/// <summary>
/// Request to update expense notes
/// </summary>

public record UpdateExpenseNotesRequest
{
    /// <summary>
    /// New expense notes
    /// </summary>
    public required string Notes { get; init; }
}