using ExpenseManager.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ExpenseManager.Tests.Domain.ValueObjects;

public class EmailTests
{
    #region Valid Email Tests

    [Fact]
    public void Should_Create_Email_With_Valid_Address()
    {
        // Arrange
        var emailAddress = "john@example.com";

        // Act
        var email = new Email(emailAddress);

        // Assert
        email.Value.Should().Be("john@example.com");
    }

    [Theory]
    [InlineData("user@domain.com")]
    [InlineData("test.user@example.co.uk")]
    [InlineData("user+tag@example.com")]
    [InlineData("user_name@example.com")]
    [InlineData("123@example.com")]
    public void Should_Accept_Valid_Email_Formats(string validEmail)
    {
        // Act
        var email = new Email(validEmail);

        // Assert
        email.Value.Should().Be(validEmail.ToLowerInvariant());
    }

    [Fact]
    public void Should_Convert_Email_To_Lowercase()
    {
        // Arrange
        var emailAddress = "John.Doe@EXAMPLE.COM";

        // Act
        var email = new Email(emailAddress);

        // Assert
        email.Value.Should().Be("john.doe@example.com");
    }

    [Fact]
    public void Should_Trim_Email_Whitespace()
    {
        // Arrange
        var emailAddress = "  john@example.com  ";

        // Act
        var email = new Email(emailAddress);

        // Assert
        email.Value.Should().Be("john@example.com");
    }

    #endregion

    #region Invalid Email Tests

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_Throw_Exception_When_Email_Is_Empty(string invalidEmail)
    {
        // Act
        Action act = () => new Email(invalidEmail);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email cannot be empty*");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@example.com")]
    [InlineData("invalid@.com")]
    [InlineData("invalid@domain")]
    [InlineData("invalid @example.com")]
    [InlineData("invalid@exam ple.com")]
    public void Should_Throw_Exception_When_Email_Format_Is_Invalid(string invalidEmail)
    {
        // Act
        Action act = () => new Email(invalidEmail);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid email format*");
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Two_Emails_With_Same_Value_Should_Be_Equal()
    {
        // Arrange
        var email1 = new Email("john@example.com");
        var email2 = new Email("john@example.com");

        // Act & Assert
        email1.Should().Be(email2);
        (email1 == email2).Should().BeTrue();
    }

    [Fact]
    public void Two_Emails_With_Different_Values_Should_Not_Be_Equal()
    {
        // Arrange
        var email1 = new Email("john@example.com");
        var email2 = new Email("jane@example.com");

        // Act & Assert
        email1.Should().NotBe(email2);
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void Emails_With_Different_Case_Should_Be_Equal()
    {
        // Arrange
        var email1 = new Email("John@Example.COM");
        var email2 = new Email("john@example.com");

        // Act & Assert
        email1.Should().Be(email2); // Porque converte para lowercase
    }

    #endregion

    #region Conversion Tests

    [Fact]
    public void Should_Implicitly_Convert_To_String()
    {
        // Arrange
        var email = new Email("john@example.com");

        // Act
        string emailString = email; // Conversão implícita

        // Assert
        emailString.Should().Be("john@example.com");
    }

    [Fact]
    public void ToString_Should_Return_Email_Value()
    {
        // Arrange
        var email = new Email("john@example.com");

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be("john@example.com");
    }

    #endregion
}