using ExpenseManager.Domain.Enums;

namespace ExpenseManager.API.DTOs.Expenses.Requests;

/// <summary>
/// Request to update expense payment method
/// </summary>

public record UpdateExpensePaymentMethodRequest
{
    /// <summary>
    /// New payment method
    /// </summary>
    public required PaymentMethod? PaymentMethod { get; init; }
}
