using Microsoft.AspNetCore.Mvc;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Domain.ValueObjects;

namespace ExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private static readonly List<Category> _categories = new();

    /// <summary>
    /// Cria uma nova categoria
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateCategory([FromBody] CreateCategoryRequest request)
    {
        try
        {
            var category = new Category(request.Name, request.UserId, request.Description);
            _categories.Add(category);
            return Ok(new
            {
                id = category.Id,
                name = category.Name,
                description = category.Description,
                userId = category.UserId,
                createdAt = category.CreatedAt
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }



    public record CreateCategoryRequest(string Name, Guid UserId, string? Description);
}
