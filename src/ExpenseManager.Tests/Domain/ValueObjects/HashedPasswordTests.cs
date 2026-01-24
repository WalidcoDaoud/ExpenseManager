using ExpenseManager.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ExpenseManager.Tests.Domain.ValueObjects;

public class HashedPasswordTests
{
    #region Constructor Tests

    [Fact]
    public void Should_Create_HashedPassword_With_Valid_Data()
    {
        // Arrange
        var hash = "hashed_password_string";
        var salt = "salt_string";

        // Act
        var password = new HashedPassword(hash, salt);

        // Assert
        password.Hash.Should().Be("hashed_password_string");
        password.Salt.Should().Be("salt_string");
    }

    [Theory]
    [InlineData("", "salt")]
    [InlineData(" ", "salt")]
    [InlineData(null, "salt")]
    public void Should_Throw_Exception_When_Hash_Is_Empty(string invalidHash, string salt) // ← Adicione 'string salt'
    {
        // Act
        Action act = () => new HashedPassword(invalidHash, salt);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Hash cannot be empty*");
    }

    [Theory]
    [InlineData("hash", "")]
    [InlineData("hash", " ")]
    [InlineData("hash", null)]
    public void Should_Throw_Exception_When_Salt_Is_Empty(string hash, string invalidSalt) // ← Já está correto
    {
        // Act
        Action act = () => new HashedPassword(hash, invalidSalt);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Salt cannot be empty*");
    }

    #endregion

    #region Equality Tests

    [Fact]
    public void Two_HashedPasswords_With_Same_Data_Should_Be_Equal()
    {
        // Arrange
        var password1 = new HashedPassword("hash123", "salt123");
        var password2 = new HashedPassword("hash123", "salt123");

        // Act & Assert
        password1.Should().Be(password2);
        (password1 == password2).Should().BeTrue();
    }

    [Fact]
    public void Two_HashedPasswords_With_Different_Hashes_Should_Not_Be_Equal()
    {
        // Arrange
        var password1 = new HashedPassword("hash123", "salt123");
        var password2 = new HashedPassword("hash456", "salt123");

        // Act & Assert
        password1.Should().NotBe(password2);
        (password1 != password2).Should().BeTrue();
    }

    [Fact]
    public void Two_HashedPasswords_With_Different_Salts_Should_Not_Be_Equal()
    {
        // Arrange
        var password1 = new HashedPassword("hash123", "salt123");
        var password2 = new HashedPassword("hash123", "salt456");

        // Act & Assert
        password1.Should().NotBe(password2);
    }

    #endregion

    #region Security Tests

    [Fact]
    public void Should_Store_Hash_And_Salt_Separately()
    {
        // Arrange & Act
        var password = new HashedPassword("myHash", "mySalt");

        // Assert
        password.Hash.Should().NotContain(password.Salt);
        password.Salt.Should().NotContain(password.Hash);
    }

    #endregion
}