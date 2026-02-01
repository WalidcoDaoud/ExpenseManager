using Microsoft.AspNetCore.Mvc;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.ValueObjects;
using ExpenseManager.Application.Interfaces;
using ExpenseManager.API.DTOs.Users.Requests;

namespace ExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                return Conflict(new { error = "Email already exists" });
            }

            var email = new Email(request.Email);

            var password = new HashedPassword(
                hash: Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(request.Password)),
                salt: "temporary-salt"
            );

            var user = new User(request.Name, email, password);

            await _userRepository.AddAsync(user);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                new
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email.Value,
                    createdAt = user.CreatedAt,
                    isActive = user.IsActive
                });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();

        var response = users.Select(u => new
        {
            id = u.Id,
            name = u.Name,
            email = u.Email.Value,
            createdAt = u.CreatedAt,
            isActive = u.IsActive,
            lastLoginAt = u.LastLoginAt
        });

        return Ok(response);
    }

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { error = "User not found" });

        return Ok(new
        {
            id = user.Id,
            name = user.Name,
            email = user.Email.Value,
            createdAt = user.CreatedAt,
            isActive = user.IsActive,
            lastLoginAt = user.LastLoginAt,
            updatedAt = user.UpdatedAt
        });
    }

    /// <summary>
    /// Updates user name
    /// </summary>
    [HttpPut("{id}/name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateName(Guid id, [FromBody] UpdateUserNameRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { error = "User not found" });

        try
        {
            user.UpdateName(request.Name);
            await _userRepository.UpdateAsync(user);

            return Ok(new
            {
                message = "Name updated successfully",
                name = user.Name,
                updatedAt = user.UpdatedAt
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deactivates a user
    /// </summary>
    [HttpPut("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { error = "User not found" });

        user.Deactivate();
        await _userRepository.UpdateAsync(user);

        return Ok(new
        {
            message = "User deactivated",
            isActive = user.IsActive,
            updatedAt = user.UpdatedAt
        });
    }

    /// <summary>
    /// Activates a user
    /// </summary>
    [HttpPut("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return NotFound(new { error = "User not found" });

        user.Activate();
        await _userRepository.UpdateAsync(user);

        return Ok(new
        {
            message = "User activated",
            isActive = user.IsActive,
            updatedAt = user.UpdatedAt
        });
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        if (!await _userRepository.ExistsAsync(id))
            return NotFound(new { error = "User not found" });

        await _userRepository.DeleteAsync(id);

        return NoContent();
    }
}