using Microsoft.AspNetCore.Mvc;
using ExpenseManager.Domain.Entities;
using ExpenseManager.API.DTOs.Categories.Requests;

namespace ExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private static readonly List<Category> _categories = new();

    /// <summary>
    /// Create a new category
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

    /// <summary>
    /// Update the name of a category
    /// </summary>
    [HttpPut("{id}/name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateName(Guid id, [FromBody] UpdateCategoryNameRequest request)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
            return NotFound(new { error = "Category not found" });
        try
        {
            category.UpdateName(request.Name);

            return Ok(new
            {
                message = "Name updated successfully",
                id = category.Id,
                name = category.Name,
                updatedAt = category.UpdatedAt
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Update the description of a categoria
    /// </summary>
    [HttpPut("{id}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateDescription(Guid id, [FromBody] UpdateCategoryDescriptionRequest request)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
            return NotFound(new { error = "Category not found" });
        try
        {
            category.UpdateDescription(request.Description);
            return Ok(new
            {
                message = "Description updated successfully",
                id = category.Id,
                description = category.Description,
                updatedAt = category.UpdatedAt
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// List all Categories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllCategories()
    {
        var categories = _categories.Select(c => new
        {
            id = c.Id,
            name = c.Name,
            description = c.Description,
            userId = c.UserId,
            createdAt = c.CreatedAt,
            updatedAt = c.UpdatedAt
        });

        return Ok(categories);
    }

    /// <summary>
    /// Get a Category by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);

        if (category == null)
            return NotFound(new { error = "Category not found" });

        return Ok(new
        {
            id = category.Id,
            name = category.Name,
            description = category.Description,
            userId = category.UserId,
            createdAt = category.CreatedAt,
            updatedAt = category.UpdatedAt
        });
    }

    /// <summary>
    /// List all Categories of a specific user
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetByUserId(Guid userId)
    {
        var categories = _categories
            .Where(c => c.UserId == userId)
            .Select(c => new
            {
                id = c.Id,
                name = c.Name,
                description = c.Description,
                createdAt = c.CreatedAt
            });

        return Ok(categories);
    }
}
