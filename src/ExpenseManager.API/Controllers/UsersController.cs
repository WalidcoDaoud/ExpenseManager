using Microsoft.AspNetCore.Mvc;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.ValueObjects;

namespace ExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // Lista em memória (temporário para teste)
    private static readonly List<User> _users = new();

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var email = new Email(request.Email);

            // Hash temporário (vamos melhorar depois)
            var password = new HashedPassword(
                hash: Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(request.Password)),
                salt: "temp-salt"
            );

            var user = new User(request.Name, email, password);
            _users.Add(user);

            return Ok(new
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
    /// Lista todos os usuários
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllUsers()
    {
        var users = _users.Select(u => new
        {
            id = u.Id,
            name = u.Name,
            email = u.Email.Value,
            createdAt = u.CreatedAt,
            isActive = u.IsActive,
            lastLoginAt = u.LastLoginAt
        });

        return Ok(users);
    }

    /// <summary>
    /// Busca um usuário por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUserById(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound(new { error = "User not found" });

        return Ok(new
        {
            id = user.Id,
            name = user.Name,
            email = user.Email.Value,
            createdAt = user.CreatedAt,
            isActive = user.IsActive,
            lastLoginAt = user.LastLoginAt
        });
    }

    /// <summary>
    /// Desativa um usuário
    /// </summary>
    [HttpPut("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeactivateUser(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound(new { error = "User not found" });

        user.Deactivate();

        return Ok(new
        {
            message = "User deactivated",
            isActive = user.IsActive,
            updatedAt = user.UpdatedAt
        });
    }

    /// <summary>
    /// Ativa um usuário
    /// </summary>
    [HttpPut("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ActivateUser(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound(new { error = "User not found" });

        user.Activate();

        return Ok(new
        {
            message = "User activated",
            isActive = user.IsActive,
            updatedAt = user.UpdatedAt
        });
    }

    /// <summary>
    /// Atualiza o nome do usuário
    /// </summary>
    [HttpPut("{id}/name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateName(Guid id, [FromBody] UpdateNameRequest request)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            return NotFound(new { error = "User not found" });

        try
        {
            user.UpdateName(request.Name);

            return Ok(new
            {
                message = "Name updated",
                name = user.Name,
                updatedAt = user.UpdatedAt
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

// DTOs (Data Transfer Objects)
public record CreateUserRequest(string Name, string Email, string Password);
public record UpdateNameRequest(string Name);