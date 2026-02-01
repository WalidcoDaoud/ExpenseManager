namespace ExpenseManager.Domain.Enums;

/// <summary>
/// Payment method used for the transaction
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Physical cash payment
    /// </summary>
    Cash = 0,

    /// <summary>
    /// Debit card payment
    /// </summary>
    DebitCard = 1,

    /// <summary>
    /// Credit card payment
    /// </summary>
    CreditCard = 2,

    /// <summary>
    /// PIX instant payment (Brazil)
    /// </summary>
    Pix = 3,

    /// <summary>
    /// Bank transfer or wire transfer
    /// </summary>
    BankTransfer = 4,

    /// <summary>
    /// Other payment methods
    /// </summary>
    Other = 99
}