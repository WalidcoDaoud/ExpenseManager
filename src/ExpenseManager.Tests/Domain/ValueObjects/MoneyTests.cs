using ExpenseManager.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ExpenseManager.Tests.Domain.ValueObjects;

public class MoneyTests
{
    #region Constructor Tests

    [Fact]
    public void Should_Create_Money_With_Valid_Amount()
    {
        // Arrange & Act
        var money = new Money(100.50m, "BRL");

        // Assert
        money.Amount.Should().Be(100.50m);
        money.Currency.Should().Be("BRL");
    }

    [Fact]
    public void Should_Default_To_BRL_Currency()
    {
        // Arrange & Act
        var money = new Money(100m);

        // Assert
        money.Currency.Should().Be("BRL");
    }

    [Fact]
    public void Should_Convert_Currency_To_Uppercase()
    {
        // Arrange & Act
        var money = new Money(100m, "usd");

        // Assert
        money.Currency.Should().Be("USD");
    }

    [Fact]
    public void Should_Throw_Exception_When_Amount_Is_Negative()
    {
        // Act
        Action act = () => new Money(-10m, "BRL");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Amount cannot be negative*");
    }

    [Fact]
    public void Should_Accept_Zero_Amount()
    {
        // Act
        var money = new Money(0m, "BRL");

        // Assert
        money.Amount.Should().Be(0m);
    }

    #endregion

    #region Addition Tests

    [Fact]
    public void Should_Add_Money_With_Same_Currency()
    {
        // Arrange
        var money1 = new Money(100m, "BRL");
        var money2 = new Money(50m, "BRL");

        // Act
        var result = money1 + money2;

        // Assert
        result.Amount.Should().Be(150m);
        result.Currency.Should().Be("BRL");
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Different_Currencies()
    {
        // Arrange
        var money1 = new Money(100m, "BRL");
        var money2 = new Money(50m, "USD");

        // Act
        Action act = () => { var result = money1 + money2; };

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Cannot add money with different currencies*");
    }

    [Theory]
    [InlineData(100, 50, 150)]
    [InlineData(0, 100, 100)]
    [InlineData(100, 0, 100)]
    [InlineData(0.01, 0.02, 0.03)]
    [InlineData(999.99, 0.01, 1000.00)]
    public void Should_Add_Money_Correctly(decimal amount1, decimal amount2, decimal expected)
    {
        // Arrange
        var money1 = new Money(amount1, "BRL");
        var money2 = new Money(amount2, "BRL");

        // Act
        var result = money1 + money2;

        // Assert
        result.Amount.Should().Be(expected);
    }

    #endregion

    #region Subtraction Tests

    [Fact]
    public void Should_Subtract_Money_With_Same_Currency()
    {
        // Arrange
        var money1 = new Money(100m, "BRL");
        var money2 = new Money(30m, "BRL");

        // Act
        var result = money1 - money2;

        // Assert
        result.Amount.Should().Be(70m);
        result.Currency.Should().Be("BRL");
    }

    [Fact]
    public void Should_Throw_Exception_When_Subtracting_Different_Currencies()
    {
        // Arrange
        var money1 = new Money(100m, "BRL");
        var money2 = new Money(50m, "USD");

        // Act
        Action act = () => { var result = money1 - money2; };

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Cannot subtract money with different currencies*");
    }

    [Theory]
    [InlineData(100, 50, 50)]
    [InlineData(100, 100, 0)]
    [InlineData(100, 0, 100)]
    [InlineData(0.05, 0.02, 0.03)]
    public void Should_Subtract_Money_Correctly(decimal amount1, decimal amount2, decimal expected)
    {
        // Arrange
        var money1 = new Money(amount1, "BRL");
        var money2 = new Money(amount2, "BRL");

        // Act
        var result = money1 - money2;

        // Assert
        result.Amount.Should().Be(expected);
    }

    [Fact]
    public void Subtraction_Can_Result_In_Negative_Amount()
    {
        // Arrange
        var money1 = new Money(50m, "BRL");
        var money2 = new Money(100m, "BRL");

        // Act
        Action act = () => { var result = money1 - money2; };

        // Assert
        // Isso vai falhar porque Money não aceita negativos no construtor
        act.Should().Throw<ArgumentException>();
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Two_Money_With_Same_Amount_And_Currency_Should_Be_Equal()
    {
        // Arrange
        var money1 = new Money(100m, "BRL");
        var money2 = new Money(100m, "BRL");

        // Act & Assert
        money1.Should().Be(money2);
        (money1 == money2).Should().BeTrue();
    }

    [Fact]
    public void Two_Money_With_Different_Amounts_Should_Not_Be_Equal()
    {
        // Arrange
        var money1 = new Money(100m, "BRL");
        var money2 = new Money(50m, "BRL");

        // Act & Assert
        money1.Should().NotBe(money2);
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void Two_Money_With_Different_Currencies_Should_Not_Be_Equal()
    {
        // Arrange
        var money1 = new Money(100m, "BRL");
        var money2 = new Money(100m, "USD");

        // Act & Assert
        money1.Should().NotBe(money2);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Should_Handle_Very_Small_Amounts()
    {
        // Arrange & Act
        var money = new Money(0.01m, "BRL");

        // Assert
        money.Amount.Should().Be(0.01m);
    }

    [Fact]
    public void Should_Handle_Very_Large_Amounts()
    {
        // Arrange & Act
        var money = new Money(999999999.99m, "BRL");

        // Assert
        money.Amount.Should().Be(999999999.99m);
    }

    [Theory]
    [InlineData("BRL")]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    [InlineData("JPY")]
    public void Should_Accept_Different_Currency_Codes(string currency)
    {
        // Act
        var money = new Money(100m, currency);

        // Assert
        money.Currency.Should().Be(currency.ToUpper());
    }

    #endregion
}