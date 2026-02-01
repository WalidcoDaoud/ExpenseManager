namespace ExpenseManager.API.DTOs.Expenses.Requests;

/// <summary>
/// Request to update expense amount
/// </summary>

public record UpdateExpenseAmountRequest
{
    /// <summary>
    /// New expense amount
    /// </summary>
    public required decimal Amount { get; init; }

    /// <summary>
    /// New currency
    /// </summary>
    public string Currency { get; init; } = "BRL";
}
