using ExpenseManager.Domain.Common;
using ExpenseManager.Domain.ValueObjects;

namespace ExpenseManager.Domain.Entities;

public class User : Entity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public HashedPassword Password { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public User(string name, Email email, HashedPassword password)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (name.Length < 3)
            throw new ArgumentException("Name must have at least 3 characters", nameof(name));

        Name = name.Trim();
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        IsActive = true;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty", nameof(newName));

        if (newName.Length < 3)
            throw new ArgumentException("Name must have at least 3 characters", nameof(newName));

        Name = newName.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(Email newEmail)
    {
        Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(HashedPassword newPassword)
    {
        Password = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }
}