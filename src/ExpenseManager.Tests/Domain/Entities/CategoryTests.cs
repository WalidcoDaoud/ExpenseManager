using ExpenseManager.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ExpenseManager.Tests.Domain.Entities;

public class CategoryTests
{
    [Fact]
    public void Should_Create_Category_With_Valid_Data()
    {
        // Arrange
        var name = "Food";
        var userId = Guid.NewGuid();
        var description = "Restaurant expenses";

        // Act
        var category = new Category(name, userId, description);

        // Assert
        category.Name.Should().Be("Food");
        category.UserId.Should().Be(userId);
        category.Description.Should().Be("Restaurant expenses");
        category.Id.Should().NotBe(Guid.Empty);
        category.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        category.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Should_Create_Category_Without_Description()
    {
        // Arrange
        var name = "Food";
        var userId = Guid.NewGuid();

        // Act
        var category = new Category(name, userId);

        // Assert
        category.Name.Should().Be("Food");
        category.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_Throw_Exception_When_Name_Is_Empty(string invalidName)
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        Action act = () => new Category(invalidName, userId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name cannot be empty*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Too_Short()
    {
        // Arrange
        var name = "AB";
        var userId = Guid.NewGuid();

        // Act
        Action act = () => new Category(name, userId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name must have at least 3 characters*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Too_Long()
    {
        // Arrange
        var name = new string('A', 101);
        var userId = Guid.NewGuid();

        // Act
        Action act = () => new Category(name, userId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Name cannot exceed 100 characters*");
    }

    [Fact]
    public void Should_Throw_Exception_When_UserId_Is_Empty()
    {
        // Arrange
        var name = "Food";
        var userId = Guid.Empty;

        // Act
        Action act = () => new Category(name, userId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*UserId cannot be empty*");
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Is_Too_Long()
    {
        // Arrange
        var name = "Food";
        var userId = Guid.NewGuid();
        var description = new string('A', 251);

        // Act
        Action act = () => new Category(name, userId, description);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Description cannot exceed 250 characters*");
    }

    [Fact]
    public void Should_Update_Name_Successfully()
    {
        // Arrange
        var category = new Category("Food", Guid.NewGuid());
        var newName = "Restaurants";

        // Act
        category.UpdateName(newName);

        // Assert
        category.Name.Should().Be("Restaurants");
        category.UpdatedAt.Should().NotBeNull();
        category.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Should_Throw_Exception_When_Updating_To_Invalid_Name()
    {
        // Arrange
        var category = new Category("Food", Guid.NewGuid());

        // Act
        Action act = () => category.UpdateName("");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_Update_Description_Successfully()
    {
        // Arrange
        var category = new Category("Food", Guid.NewGuid());
        var newDescription = "Updated description";

        // Act
        category.UpdateDescription(newDescription);

        // Assert
        category.Description.Should().Be("Updated description");
        category.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Should_Allow_Null_Description_On_Update()
    {
        // Arrange
        var category = new Category("Food", Guid.NewGuid(), "Old description");

        // Act
        category.UpdateDescription(null);

        // Assert
        category.Description.Should().BeNull();
    }

    [Fact]
    public void Should_Trim_Name_On_Creation()
    {
        // Arrange
        var name = "  Food  ";
        var userId = Guid.NewGuid();

        // Act
        var category = new Category(name, userId);

        // Assert
        category.Name.Should().Be("Food");
    }

    [Fact]
    public void Should_Trim_Description_On_Creation()
    {
        // Arrange
        var name = "Food";
        var userId = Guid.NewGuid();
        var description = "  Restaurant expenses  ";

        // Act
        var category = new Category(name, userId, description);

        // Assert
        category.Description.Should().Be("Restaurant expenses");
    }
}