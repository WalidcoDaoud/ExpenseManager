using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.Enums;
using ExpenseManager.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ExpenseManager.Tests.Domain.Entities;

public class ExpenseTests
{
    #region Constructor Tests - Valid Cases

    [Fact]
    public void Should_Create_Expense_With_Valid_Data()
    {
        // Arrange
        var description = "Lunch at restaurant";
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Description.Should().Be("Lunch at restaurant");
        expense.Amount.Should().Be(amount);
        expense.Amount.Amount.Should().Be(50m);
        expense.Amount.Currency.Should().Be("BRL");
        expense.Date.Should().BeCloseTo(date, TimeSpan.FromSeconds(1));
        expense.UserId.Should().Be(userId);
        expense.CategoryId.Should().Be(categoryId);
        expense.Type.Should().Be(ExpenseType.Expense); // Default value
        expense.PaymentMethod.Should().BeNull(); // Not provided
        expense.Notes.Should().BeNull(); // Not provided
        expense.Id.Should().NotBe(Guid.Empty);
        expense.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        expense.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Should_Create_Expense_With_All_Optional_Parameters()
    {
        // Arrange
        var description = "Dinner with friends";
        var amount = new Money(100m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var type = ExpenseType.Expense;
        var paymentMethod = PaymentMethod.CreditCard;
        var notes = "Split bill with 3 people";

        // Act
        var expense = new Expense(
            description,
            amount,
            date,
            userId,
            categoryId,
            type,
            paymentMethod,
            notes
        );

        // Assert
        expense.Description.Should().Be("Dinner with friends");
        expense.Type.Should().Be(ExpenseType.Expense);
        expense.PaymentMethod.Should().Be(PaymentMethod.CreditCard);
        expense.Notes.Should().Be("Split bill with 3 people");
    }

    [Fact]
    public void Should_Create_Income_Transaction()
    {
        // Arrange
        var description = "Monthly salary";
        var amount = new Money(5000m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(
            description,
            amount,
            date,
            userId,
            categoryId,
            type: ExpenseType.Income
        );

        // Assert
        expense.Type.Should().Be(ExpenseType.Income);
        expense.Amount.Amount.Should().Be(5000m);
    }

    [Theory]
    [InlineData(PaymentMethod.Cash)]
    [InlineData(PaymentMethod.DebitCard)]
    [InlineData(PaymentMethod.CreditCard)]
    [InlineData(PaymentMethod.Pix)]
    [InlineData(PaymentMethod.BankTransfer)]
    [InlineData(PaymentMethod.Other)]
    public void Should_Accept_All_Payment_Methods(PaymentMethod method)
    {
        // Arrange
        var description = "Test expense";
        var amount = new Money(10m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(
            description,
            amount,
            date,
            userId,
            categoryId,
            paymentMethod: method
        );

        // Assert
        expense.PaymentMethod.Should().Be(method);
    }

    [Fact]
    public void Should_Trim_Description_On_Creation()
    {
        // Arrange
        var description = "  Lunch at restaurant  ";
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Description.Should().Be("Lunch at restaurant"); // Trimmed
    }

    [Fact]
    public void Should_Trim_Notes_On_Creation()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(10m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var notes = "  Important note  ";

        // Act
        var expense = new Expense(
            description,
            amount,
            date,
            userId,
            categoryId,
            notes: notes
        );

        // Assert
        expense.Notes.Should().Be("Important note"); // Trimmed
    }

    #endregion

    #region Constructor Tests - Description Validation

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_Throw_Exception_When_Description_Is_Empty(string invalidDescription)
    {
        // Arrange
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Expense(
            invalidDescription,
            amount,
            date,
            userId,
            categoryId
        );

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description cannot be empty*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Is_Too_Short()
    {
        // Arrange
        var description = "AB"; // Less than 3 characters
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Expense(description, amount, date, userId, categoryId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description must have at least 3 characters*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Is_Too_Long()
    {
        // Arrange
        var description = new string('A', 201); // More than 200 characters
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Expense(description, amount, date, userId, categoryId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description cannot exceed 200 characters*");
    }

    [Fact]
    public void Should_Accept_Description_With_Exactly_3_Characters()
    {
        // Arrange
        var description = "ABC"; // Minimum valid
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Description.Should().Be("ABC");
    }

    [Fact]
    public void Should_Accept_Description_With_Exactly_200_Characters()
    {
        // Arrange
        var description = new string('A', 200); // Maximum valid
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Description.Length.Should().Be(200);
    }

    #endregion

    #region Constructor Tests - Amount Validation

    [Fact]
    public void Should_Throw_Exception_When_Amount_Is_Null()
    {
        // Arrange
        var description = "Test";
        Money amount = null;
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Expense(description, amount, date, userId, categoryId);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("amount");
    }

    [Fact]
    public void Should_Throw_Exception_When_Amount_Is_Zero()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(0m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Expense(description, amount, date, userId, categoryId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Amount must be greater than zero*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Amount_Is_Negative()
    {
        // Arrange
        var description = "Test";
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Money(-50m, "BRL"); // Money validates this

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Amount cannot be negative*");
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999999.99)]
    public void Should_Accept_Valid_Positive_Amounts(decimal validAmount)
    {
        // Arrange
        var description = "Test";
        var amount = new Money(validAmount, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Amount.Amount.Should().Be(validAmount);
    }

    #endregion

    #region Constructor Tests - Date Validation

    [Fact]
    public void Should_Accept_Current_Date()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Date.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Should_Accept_Date_One_Day_In_Future()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow.AddDays(1);
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Date.Should().BeCloseTo(date, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Should_Throw_Exception_When_Date_Is_Too_Far_In_Future()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow.AddDays(2); // More than 1 day
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Expense(description, amount, date, userId, categoryId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Date cannot be more than 1 day in the future*");
    }

    [Fact]
    public void Should_Accept_Past_Dates()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow.AddYears(-1); // 1 year ago
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Date.Should().BeCloseTo(date, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Should_Throw_Exception_When_Date_Is_Before_Year_2000()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(50m, "BRL");
        var date = new DateTime(1999, 12, 31); // Before 2000
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Expense(description, amount, date, userId, categoryId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Date cannot be before year 2000*");
    }

    [Fact]
    public void Should_Accept_Date_At_Year_2000_Boundary()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(50m, "BRL");
        var date = new DateTime(2000, 1, 1); // Exactly 2000-01-01
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Act
        var expense = new Expense(description, amount, date, userId, categoryId);

        // Assert
        expense.Date.Should().Be(date);
    }

    #endregion

    #region Constructor Tests - UserId and CategoryId Validation

    [Fact]
    public void Should_Throw_Exception_When_UserId_Is_Empty()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.Empty;
        var categoryId = Guid.NewGuid();

        // Act
        Action act = () => new Expense(description, amount, date, userId, categoryId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*UserId cannot be empty*");
    }

    [Fact]
    public void Should_Throw_Exception_When_CategoryId_Is_Empty()
    {
        // Arrange
        var description = "Test";
        var amount = new Money(50m, "BRL");
        var date = DateTime.UtcNow;
        var userId = Guid.NewGuid();
        var categoryId = Guid.Empty;

        // Act
        Action act = () => new Expense(description, amount, date, userId, categoryId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*CategoryId cannot be empty*");
    }

    #endregion

    #region UpdateDescription Tests

    [Fact]
    public void Should_Update_Description_Successfully()
    {
        // Arrange
        var expense = CreateValidExpense();
        var newDescription = "Updated description";

        // Act
        expense.UpdateDescription(newDescription);

        // Assert
        expense.Description.Should().Be("Updated description");
        expense.UpdatedAt.Should().NotBeNull();
        expense.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_Throw_Exception_When_Updating_To_Empty_Description(string invalidDescription)
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        Action act = () => expense.UpdateDescription(invalidDescription);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description cannot be empty*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_To_Short_Description()
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        Action act = () => expense.UpdateDescription("AB");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description must have at least 3 characters*");
    }

    [Fact]
    public void Should_Trim_Description_On_Update()
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        expense.UpdateDescription("  New description  ");

        // Assert
        expense.Description.Should().Be("New description");
    }

    #endregion

    #region UpdateAmount Tests

    [Fact]
    public void Should_Update_Amount_Successfully()
    {
        // Arrange
        var expense = CreateValidExpense();
        var newAmount = new Money(100m, "BRL");

        // Act
        expense.UpdateAmount(newAmount);

        // Assert
        expense.Amount.Should().Be(newAmount);
        expense.Amount.Amount.Should().Be(100m);
        expense.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_To_Null_Amount()
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        Action act = () => expense.UpdateAmount(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("amount");
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_To_Zero_Amount()
    {
        // Arrange
        var expense = CreateValidExpense();
        var zeroAmount = new Money(0m, "BRL");

        // Act
        Action act = () => expense.UpdateAmount(zeroAmount);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Amount must be greater than zero*");
    }

    #endregion

    #region UpdateDate Tests

    [Fact]
    public void Should_Update_Date_Successfully()
    {
        // Arrange
        var expense = CreateValidExpense();
        var newDate = DateTime.UtcNow.AddDays(-5);

        // Act
        expense.UpdateDate(newDate);

        // Assert
        expense.Date.Should().BeCloseTo(newDate, TimeSpan.FromSeconds(1));
        expense.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_To_Invalid_Future_Date()
    {
        // Arrange
        var expense = CreateValidExpense();
        var invalidDate = DateTime.UtcNow.AddDays(5);

        // Act
        Action act = () => expense.UpdateDate(invalidDate);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Date cannot be more than 1 day in the future*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_To_Date_Before_2000()
    {
        // Arrange
        var expense = CreateValidExpense();
        var invalidDate = new DateTime(1999, 1, 1);

        // Act
        Action act = () => expense.UpdateDate(invalidDate);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Date cannot be before year 2000*");
    }

    #endregion

    #region ChangeCategory Tests

    [Fact]
    public void Should_Change_Category_Successfully()
    {
        // Arrange
        var expense = CreateValidExpense();
        var newCategoryId = Guid.NewGuid();

        // Act
        expense.ChangeCategory(newCategoryId);

        // Assert
        expense.CategoryId.Should().Be(newCategoryId);
        expense.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Throw_Exception_When_Changing_To_Empty_CategoryId()
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        Action act = () => expense.ChangeCategory(Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*CategoryId cannot be empty*");
    }

    #endregion

    #region UpdatePaymentMethod Tests

    [Fact]
    public void Should_Update_PaymentMethod_Successfully()
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        expense.UpdatePaymentMethod(PaymentMethod.CreditCard);

        // Assert
        expense.PaymentMethod.Should().Be(PaymentMethod.CreditCard);
        expense.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Allow_Setting_PaymentMethod_To_Null()
    {
        // Arrange
        var expense = CreateValidExpense();
        expense.UpdatePaymentMethod(PaymentMethod.Cash);

        // Act
        expense.UpdatePaymentMethod(null);

        // Assert
        expense.PaymentMethod.Should().BeNull();
    }

    [Theory]
    [InlineData(PaymentMethod.Cash)]
    [InlineData(PaymentMethod.DebitCard)]
    [InlineData(PaymentMethod.CreditCard)]
    [InlineData(PaymentMethod.Pix)]
    public void Should_Update_To_All_Payment_Methods(PaymentMethod method)
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        expense.UpdatePaymentMethod(method);

        // Assert
        expense.PaymentMethod.Should().Be(method);
    }

    #endregion

    #region UpdateNotes Tests

    [Fact]
    public void Should_Update_Notes_Successfully()
    {
        // Arrange
        var expense = CreateValidExpense();
        var newNotes = "Updated notes";

        // Act
        expense.UpdateNotes(newNotes);

        // Assert
        expense.Notes.Should().Be("Updated notes");
        expense.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Allow_Setting_Notes_To_Null()
    {
        // Arrange
        var expense = CreateValidExpense();
        expense.UpdateNotes("Some notes");

        // Act
        expense.UpdateNotes(null);

        // Assert
        expense.Notes.Should().BeNull();
    }

    [Fact]
    public void Should_Trim_Notes_On_Update()
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        expense.UpdateNotes("  Important note  ");

        // Assert
        expense.Notes.Should().Be("Important note");
    }

    [Fact]
    public void Should_Allow_Empty_String_Notes()
    {
        // Arrange
        var expense = CreateValidExpense();

        // Act
        expense.UpdateNotes("");

        // Assert
        expense.Notes.Should().BeEmpty();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Creates a valid expense for testing update methods
    /// </summary>
    private Expense CreateValidExpense()
    {
        return new Expense(
            "Test expense",
            new Money(50m, "BRL"),
            DateTime.UtcNow,
            Guid.NewGuid(),
            Guid.NewGuid()
        );
    }

    #endregion
}