namespace UCConverter.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;

/// <summary>
/// Controller for unit conversion endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConvertController : ControllerBase
{
    private readonly IUnitConverterService _unitConverterService;
    private readonly ILocalizationService _localizationService;
    private readonly ILogger<ConvertController> _logger;

    public ConvertController(
        IUnitConverterService unitConverterService, 
        ILocalizationService localizationService,
        ILogger<ConvertController> logger)
    {
        _unitConverterService = unitConverterService ?? throw new ArgumentNullException(nameof(unitConverterService));
        _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Convert a value from one unit to another
    /// </summary>
    /// <param name="request">Conversion request</param>
    /// <returns>Conversion result</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ConvertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConvertResponseDto>> Convert([FromBody] ConvertRequestDto request)
    {
        if (request == null)
        {
            return BadRequest(new { error = "Request body is required" });
        }

        if (string.IsNullOrWhiteSpace(request.Category))
        {
            return BadRequest(new { error = "Category is required" });
        }

        if (string.IsNullOrWhiteSpace(request.FromUnit))
        {
            return BadRequest(new { error = "FromUnit is required" });
        }

        if (string.IsNullOrWhiteSpace(request.ToUnit))
        {
            return BadRequest(new { error = "ToUnit is required" });
        }

        try
        {
            var result = await _unitConverterService.ConvertAsync(request);
            return Ok(result);
        }
        catch (CategoryNotFoundException ex)
        {
            _logger.LogWarning("Category not found: {Category}", ex.CategoryName);
            var errorMessage = _localizationService.GetErrorMessage("CategoryNotFound", ex.CategoryName);
            return NotFound(new { error = errorMessage, category = ex.CategoryName });
        }
        catch (UnitNotFoundException ex)
        {
            _logger.LogWarning("Unit not found: {Unit}", ex.UnitSymbol);
            var errorMessage = _localizationService.GetErrorMessage("UnitNotFound", ex.UnitSymbol);
            return NotFound(new { error = errorMessage, unit = ex.UnitSymbol });
        }
        catch (InvalidConversionException ex)
        {
            _logger.LogWarning("Invalid conversion: {From} to {To}", ex.FromUnit, ex.ToUnit);
            var errorMessage = _localizationService.GetErrorMessage("InvalidConversion");
            return BadRequest(new { error = errorMessage, fromUnit = ex.FromUnit, toUnit = ex.ToUnit });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing conversion");
            return StatusCode(500, new { error = "An error occurred while performing the conversion" });
        }
    }
}

