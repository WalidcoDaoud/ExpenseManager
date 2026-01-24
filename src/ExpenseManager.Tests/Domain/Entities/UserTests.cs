using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ExpenseManager.Tests.Domain.Entities;

public class UserTests
{
    #region Constructor Tests

    [Fact]
    public void Should_Create_User_With_Valid_Data()
    {
        // Arrange
        var name = "John Doe";
        var email = new Email("john@example.com");
        var password = new HashedPassword("hashedPassword123", "salt123");

        // Act
        var user = new User(name, email, password);

        // Assert
        user.Name.Should().Be("John Doe");
        user.Email.Should().Be(email);
        user.Email.Value.Should().Be("john@example.com");
        user.Password.Should().Be(password);
        user.IsActive.Should().BeTrue();
        user.LastLoginAt.Should().BeNull();
        user.Id.Should().NotBe(Guid.Empty);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_Throw_Exception_When_Name_Is_Empty(string invalidName)
    {
        // Arrange
        var email = new Email("john@example.com");
        var password = new HashedPassword("hash", "salt");

        // Act
        Action act = () => new User(invalidName, email, password);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name cannot be empty*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Too_Short()
    {
        // Arrange
        var name = "Jo"; // Menos de 3 caracteres
        var email = new Email("john@example.com");
        var password = new HashedPassword("hash", "salt");

        // Act
        Action act = () => new User(name, email, password);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name must have at least 3 characters*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Email_Is_Null()
    {
        // Arrange
        var name = "John Doe";
        Email email = null;
        var password = new HashedPassword("hash", "salt");

        // Act
        Action act = () => new User(name, email, password);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("email");
    }

    [Fact]
    public void Should_Throw_Exception_When_Password_Is_Null()
    {
        // Arrange
        var name = "John Doe";
        var email = new Email("john@example.com");
        HashedPassword password = null;

        // Act
        Action act = () => new User(name, email, password);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("password");
    }

    [Fact]
    public void Should_Trim_Name_On_Creation()
    {
        // Arrange
        var name = "  John Doe  ";
        var email = new Email("john@example.com");
        var password = new HashedPassword("hash", "salt");

        // Act
        var user = new User(name, email, password);

        // Assert
        user.Name.Should().Be("John Doe"); // Sem espaços
    }

    #endregion

    #region UpdateName Tests

    [Fact]
    public void Should_Update_Name_Successfully()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );
        var newName = "John Peter Doe";

        // Act
        user.UpdateName(newName);

        // Assert
        user.Name.Should().Be("John Peter Doe");
        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_Throw_Exception_When_Updating_To_Empty_Name(string invalidName)
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Act
        Action act = () => user.UpdateName(invalidName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name cannot be empty*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_To_Short_Name()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Act
        Action act = () => user.UpdateName("Jo");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name must have at least 3 characters*");
    }

    [Fact]
    public void Should_Trim_Name_On_Update()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Act
        user.UpdateName("  Jane Doe  ");

        // Assert
        user.Name.Should().Be("Jane Doe"); // Sem espaços
    }

    #endregion

    #region UpdateEmail Tests

    [Fact]
    public void Should_Update_Email_Successfully()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );
        var newEmail = new Email("newemail@example.com");

        // Act
        user.UpdateEmail(newEmail);

        // Assert
        user.Email.Should().Be(newEmail);
        user.Email.Value.Should().Be("newemail@example.com");
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_To_Null_Email()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Act
        Action act = () => user.UpdateEmail(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("newEmail");
    }

    #endregion

    #region ChangePassword Tests

    [Fact]
    public void Should_Change_Password_Successfully()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("oldHash", "oldSalt")
        );
        var newPassword = new HashedPassword("newHash", "newSalt");

        // Act
        user.ChangePassword(newPassword);

        // Assert
        user.Password.Should().Be(newPassword);
        user.Password.Hash.Should().Be("newHash");
        user.Password.Salt.Should().Be("newSalt");
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Throw_Exception_When_Changing_To_Null_Password()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Act
        Action act = () => user.ChangePassword(null);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("newPassword");
    }

    #endregion

    #region Activate/Deactivate Tests

    [Fact]
    public void Should_Deactivate_User()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Activate_User()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );
        user.Deactivate(); // Desativa primeiro

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
        user.UpdatedAt.Should().NotBeNull();
    }

    #endregion

    #region RecordLogin Tests

    [Fact]
    public void Should_Record_Login_Time()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Act
        user.RecordLogin();

        // Assert
        user.LastLoginAt.Should().NotBeNull();
        user.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Should_Update_LastLogin_On_Multiple_Logins()
    {
        // Arrange
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );
        user.RecordLogin();
        var firstLogin = user.LastLoginAt;

        // Wait a bit
        Thread.Sleep(100);

        // Act
        user.RecordLogin();

        // Assert
        user.LastLoginAt.Should().NotBe(firstLogin);
        user.LastLoginAt.Should().BeAfter(firstLogin.Value);
    }

    #endregion

    #region User State Tests

    [Fact]
    public void New_User_Should_Be_Active_By_Default()
    {
        // Arrange & Act
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Assert
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void New_User_Should_Have_No_Last_Login()
    {
        // Arrange & Act
        var user = new User(
            "John Doe",
            new Email("john@example.com"),
            new HashedPassword("hash", "salt")
        );

        // Assert
        user.LastLoginAt.Should().BeNull();
    }

    #endregion
}