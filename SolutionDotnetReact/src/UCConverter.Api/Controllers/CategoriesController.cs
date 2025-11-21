namespace UCConverter.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;

/// <summary>
/// Controller for category-related endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IUnitConverterService _unitConverterService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(IUnitConverterService unitConverterService, ILogger<CategoriesController> logger)
    {
        _unitConverterService = unitConverterService ?? throw new ArgumentNullException(nameof(unitConverterService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all available unit categories
    /// </summary>
    /// <returns>List of categories</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        try
        {
            var categories = await _unitConverterService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories");
            return StatusCode(500, new { error = "An error occurred while retrieving categories" });
        }
    }

    /// <summary>
    /// Get all units for a specific category
    /// </summary>
    /// <param name="name">Category name</param>
    /// <returns>List of units in the category</returns>
    [HttpGet("{name}/units")]
    [ProducesResponseType(typeof(IEnumerable<UnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<UnitDto>>> GetUnitsByCategory(string name)
    {
        try
        {
            var units = await _unitConverterService.GetUnitsByCategoryAsync(name);
            
            if (!units.Any())
            {
                return NotFound(new { error = "Category not found", category = name });
            }

            return Ok(units);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving units for category: {Category}", name);
            return StatusCode(500, new { error = "An error occurred while retrieving units" });
        }
    }
}

