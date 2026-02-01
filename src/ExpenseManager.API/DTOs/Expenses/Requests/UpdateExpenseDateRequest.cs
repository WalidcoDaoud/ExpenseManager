namespace ExpenseManager.API.DTOs.Expenses.Requests;

/// <summary>
/// Request to update expense date
/// </summary>

public record UpdateExpenseDateRequest
{
    /// <summary>
    /// New expense date
    /// </summary>
    public required DateTime Date { get; init; }
}
