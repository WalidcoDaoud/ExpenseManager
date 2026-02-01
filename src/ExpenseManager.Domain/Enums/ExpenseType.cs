namespace ExpenseManager.Domain.Enums;

/// <summary>
/// Represents the type of financial transaction
/// </summary>
public enum ExpenseType
{
    /// <summary>
    /// Money spent (outgoing transaction)
    /// </summary>
    Expense = 0,

    /// <summary>
    /// Money received (incoming transaction)
    /// </summary>
    Income = 1
}
