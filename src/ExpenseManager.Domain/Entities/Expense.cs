using ExpenseManager.Domain.Common;
using ExpenseManager.Domain.Enums;
using ExpenseManager.Domain.ValueObjects;

namespace ExpenseManager.Domain.Entities;

/// <summary>
/// Represents a financial transaction (expense or income)
/// </summary>
public class Expense : Entity
{
    #region Properties

    /// <summary>
    /// Brief description of the transaction
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Transaction amount with currency
    /// </summary>
    public Money Amount { get; private set; }

    /// <summary>
    /// Date when the transaction occurred
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Type of transaction (Expense or Income)
    /// </summary>
    public ExpenseType Type { get; private set; }

    /// <summary>
    /// Optional payment method used
    /// </summary>
    public PaymentMethod? PaymentMethod { get; private set; }

    /// <summary>
    /// Optional additional notes
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// ID of the user who owns this transaction
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// ID of the category this transaction belongs to
    /// </summary>
    public Guid CategoryId { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// EF Core constructor (private to prevent direct instantiation)
    /// </summary>
    private Expense() { }

    /// <summary>
    /// Creates a new expense transaction
    /// </summary>
    /// <param name="description">Brief description of the transaction</param>
    /// <param name="amount">Transaction amount (must be positive)</param>
    /// <param name="date">Date when transaction occurred</param>
    /// <param name="userId">User who owns this transaction</param>
    /// <param name="categoryId">Category this transaction belongs to</param>
    /// <param name="type">Type of transaction (defaults to Expense)</param>
    /// <param name="paymentMethod">Optional payment method</param>
    /// <param name="notes">Optional additional notes</param>
    public Expense(
        string description,
        Money amount,
        DateTime date,
        Guid userId,
        Guid categoryId,
        ExpenseType type = ExpenseType.Expense,
        PaymentMethod? paymentMethod = null,
        string? notes = null)
    {
        ValidateDescription(description);
        ValidateAmount(amount);
        ValidateDate(date);
        ValidateUserId(userId);
        ValidateCategoryId(categoryId);

        Description = description.Trim();
        Amount = amount;
        Date = date;
        UserId = userId;
        CategoryId = categoryId;
        Type = type;
        PaymentMethod = paymentMethod;
        Notes = notes?.Trim();
    }

    #endregion

    #region Validation Methods

    private void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        if (description.Length < 3)
            throw new ArgumentException("Description must have at least 3 characters", nameof(description));
        if (description.Length > 200)
            throw new ArgumentException("Description cannot exceed 200 characters", nameof(description));
    }

    private void ValidateAmount(Money amount)
    {
        if (amount == null)
            throw new ArgumentNullException(nameof(amount), "Amount cannot be null");
        if (amount.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));
    }

    private void ValidateDate(DateTime date)
    {
        if (date > DateTime.UtcNow.AddDays(1))
            throw new ArgumentException("Date cannot be more than 1 day in the future", nameof(date));

        if (date < new DateTime(2000, 1, 1))
            throw new ArgumentException("Date cannot be before year 2000", nameof(date));
    }

    private void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty", nameof(userId));
    }

    private void ValidateCategoryId(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("CategoryId cannot be empty", nameof(categoryId));
    }

    #endregion

    #region Behavior Methods

    /// <summary>
    /// Updates the transaction description
    /// </summary>
    public void UpdateDescription(string newDescription)
    {
        ValidateDescription(newDescription);
        Description = newDescription.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the transaction amount
    /// </summary>
    public void UpdateAmount(Money newAmount)
    {
        ValidateAmount(newAmount);
        Amount = newAmount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the transaction date
    /// </summary>
    public void UpdateDate(DateTime newDate)
    {
        ValidateDate(newDate);
        Date = newDate;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Changes the category of this transaction
    /// </summary>
    public void ChangeCategory(Guid newCategoryId)
    {
        ValidateCategoryId(newCategoryId);
        CategoryId = newCategoryId;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the payment method (can be null)
    /// </summary>
    public void UpdatePaymentMethod(PaymentMethod? newPaymentMethod)
    {
        PaymentMethod = newPaymentMethod;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates additional notes (can be null or empty)
    /// </summary>
    public void UpdateNotes(string? newNotes)
    {
        Notes = newNotes?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    #endregion
}
