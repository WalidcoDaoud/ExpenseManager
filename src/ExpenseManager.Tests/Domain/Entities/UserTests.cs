using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ExpenseManager.Tests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Should_Throw_Excepction_When_Email_Is_Null()
    {
        // Arrange
        var name = "John Doe";
        Email email = null!;
        var password = new HashedPassword("hashed", "salt");
        // Act
        Action act = () => new User(name, email, password);
        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*email*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Password_Is_Null()
    {
        // Arrange
        var name = "John Doe";
        Email email = new Email("example@example.com");
        HashedPassword password = null!;
        //Act
        Action act = () => new User(name, email, password);
        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*password*");
    }
}
