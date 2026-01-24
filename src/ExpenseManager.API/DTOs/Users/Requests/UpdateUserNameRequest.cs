namespace ExpenseManager.API.DTOs.Users.Requests;

/// <summary>
/// Request to update user name
/// </summary>
public record UpdateUserNameRequest
{
    /// <summary>
    /// New user name
    /// </summary>
    /// <example>John Peter Doe</example>
    public required string Name { get; init; }
}