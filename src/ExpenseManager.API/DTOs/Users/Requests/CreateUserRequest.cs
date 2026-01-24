namespace ExpenseManager.API.DTOs.Users.Requests;

/// <summary>
/// Request to create a new user
/// </summary>
public record CreateUserRequest
{
    /// <summary>
    /// User name
    /// </summary>
    /// <example>John Doe</example>
    public required string Name { get; init; }

    /// <summary>
    /// User email address
    /// </summary>
    /// <example>john.doe@example.com</example>
    public required string Email { get; init; }

    /// <summary>
    /// User password
    /// </summary>
    /// <example>Password@123</example>
    public required string Password { get; init; }
}