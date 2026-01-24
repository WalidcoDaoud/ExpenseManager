using ExpenseManager.Domain.Common;

namespace ExpenseManager.Domain.Entities;

public class Category : Entity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid UserId { get; private set; }

    private Category() { }

    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (name.Length < 3)
            throw new ArgumentException("Name must have at least 3 characters", nameof(name));

        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));
    }

    public Category(string name, Guid userId, string? description = null)
    {
        ValidateName(name);

        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty", nameof(userId));

        if (!string.IsNullOrWhiteSpace(description) && description.Length > 250)
            throw new ArgumentException("Description cannot exceed 250 characters", nameof(description));

        Name = name.Trim();
        UserId = userId;
        Description = description?.Trim();
    }

    public void UpdateName(string newName)
    {
        ValidateName(newName);

        Name = newName.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string? newDescription)
    {
        if (!string.IsNullOrWhiteSpace(newDescription) && newDescription.Length > 250)
            throw new ArgumentException("Description cannot exceed 250 characters", nameof(newDescription));

        Description = newDescription?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}