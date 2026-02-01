namespace ExpenseManager.API.DTOs.Expenses.Requests;

using ExpenseManager.Domain.Enums;

/// <summary>
/// Request to create new Expense
/// </summary>

public record CreateExpenseRequest
{
    /// <summary>
    /// Brief description of the transaction
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Transaction amount (must be positive)
    ///</summary>
    public required decimal Amount { get; init; }

    /// <summary>
    /// Currency code
    /// </summary>
    public string Currency { get; init; } = "BRL";

    /// <summary>
    /// Date when the transaction occurred
    /// </summary>
    public required DateTime Date { get; init; }

    /// <summary>
    /// ID of the user who owns this transaction
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// ID of the category this transaction belongs to
    /// </summary>
    public required Guid CategoryId { get; init; }

    /// <summary>
    /// Type of transaction (0 = Expense, 1 = Income)
    /// </summary>
    public ExpenseType Type { get; init; }

    /// <summary>
    /// Optional payment method (0=Cash, 1=DebitCard, 2=CreditCard, 3=Pix, 4=BankTransfer, 99=Other)
    /// </summary>
    public PaymentMethod? PaymentMethod { get; init; }

    /// <summary>
    /// Optional additional notes
    /// </summary>
    public string? Notes { get; init; }
}