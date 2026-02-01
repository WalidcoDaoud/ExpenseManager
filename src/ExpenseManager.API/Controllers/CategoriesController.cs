using Microsoft.AspNetCore.Mvc;
using ExpenseManager.Domain.Entities;
using ExpenseManager.Application.Interfaces;
using ExpenseManager.API.DTOs.Categories.Requests;

namespace ExpenseManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;

    public CategoriesController(
        ICategoryRepository categoryRepository,
        IUserRepository userRepository)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Creates a new category
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        try
        {
            if (!await _userRepository.ExistsAsync(request.UserId))
            {
                return NotFound(new { error = "User not found" });
            }

            if (await _categoryRepository.NameExistsForUserAsync(request.UserId, request.Name))
            {
                return Conflict(new { error = "Category name already exists for this user" });
            }

            var category = new Category(request.Name, request.UserId, request.Description);

            await _categoryRepository.AddAsync(category);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = category.Id },
                new
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
    /// Gets all categories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();

        var response = categories.Select(c => new
        {
            id = c.Id,
            name = c.Name,
            description = c.Description,
            userId = c.UserId,
            createdAt = c.CreatedAt,
            updatedAt = c.UpdatedAt
        });

        return Ok(response);
    }

    /// <summary>
    /// Gets a category by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

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
    /// Gets categories by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoriesByUserId(Guid userId)
    {
        var categories = await _categoryRepository.GetByUserIdAsync(userId);

        var response = categories.Select(c => new
        {
            id = c.Id,
            name = c.Name,
            description = c.Description,
            createdAt = c.CreatedAt
        });

        return Ok(response);
    }

    /// <summary>
    /// Updates category name
    /// </summary>
    [HttpPut("{id}/name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateName(Guid id, [FromBody] UpdateCategoryNameRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            return NotFound(new { error = "Category not found" });

        try
        {
            category.UpdateName(request.Name);
            await _categoryRepository.UpdateAsync(category);

            return Ok(new
            {
                message = "Name updated successfully",
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
    /// Updates category description
    /// </summary>
    [HttpPut("{id}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] UpdateCategoryDescriptionRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            return NotFound(new { error = "Category not found" });

        try
        {
            category.UpdateDescription(request.Description);
            await _categoryRepository.UpdateAsync(category);

            return Ok(new
            {
                message = "Description updated successfully",
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
    /// Deletes a category
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        if (!await _categoryRepository.ExistsAsync(id))
            return NotFound(new { error = "Category not found" });

        await _categoryRepository.DeleteAsync(id);

        return NoContent();
    }
}