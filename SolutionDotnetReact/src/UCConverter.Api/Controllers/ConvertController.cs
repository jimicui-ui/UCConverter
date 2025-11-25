namespace UCConverter.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;

/// <summary>
/// Controller for unit conversion endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Unit Conversion")]
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
    /// <param name="request">Conversion request containing category, from unit, to unit, value, and optional locale</param>
    /// <returns>Conversion result with formatted value and unit information</returns>
    /// <remarks>
    /// Performs unit conversion within the same category. Supports both linear conversions (using conversion factors) and formula-based conversions (e.g., temperature).
    /// 
    /// **Supported Conversion Types:**
    /// - **Linear Conversions**: Most units use simple multiplication factors (e.g., length, weight, volume)
    /// - **Formula-Based Conversions**: Temperature conversions use formulas (e.g., Celsius to Fahrenheit)
    /// 
    /// **Category Examples:**
    /// - **Length**: Convert between meters, feet, inches, kilometers, miles, etc.
    /// - **Weight**: Convert between kilograms, pounds, ounces, grams, etc.
    /// - **Temperature**: Convert between Celsius, Fahrenheit, and Kelvin (formula-based)
    /// - **Volume**: Convert between liters, gallons, cubic meters, etc.
    /// - **Area**: Convert between square meters, square feet, acres, etc.
    /// - **Time**: Convert between seconds, minutes, hours, days, etc.
    /// - **Speed**: Convert between meters per second, miles per hour, kilometers per hour, etc.
    /// 
    /// **Example Requests:**
    /// 
    /// 1. **Length Conversion (Linear)**:
    /// ```json
    /// {
    ///   "category": "length",
    ///   "fromUnit": "m",
    ///   "toUnit": "ft",
    ///   "value": 10.5,
    ///   "locale": "en"
    /// }
    /// ```
    /// Result: 10.5 meters = 34.45 feet
    /// 
    /// 2. **Weight Conversion (Linear)**:
    /// ```json
    /// {
    ///   "category": "weight",
    ///   "fromUnit": "kg",
    ///   "toUnit": "lb",
    ///   "value": 5,
    ///   "locale": "en"
    /// }
    /// ```
    /// Result: 5 kilograms = 11.02 pounds
    /// 
    /// 3. **Temperature Conversion (Formula-Based)**:
    /// ```json
    /// {
    ///   "category": "temperature",
    ///   "fromUnit": "°C",
    ///   "toUnit": "°F",
    ///   "value": 25,
    ///   "locale": "en"
    /// }
    /// ```
    /// Result: 25°C = 77°F (using formula: F = C × 9/5 + 32)
    /// 
    /// 4. **Volume Conversion (Linear)**:
    /// ```json
    /// {
    ///   "category": "volume",
    ///   "fromUnit": "L",
    ///   "toUnit": "gal",
    ///   "value": 20,
    ///   "locale": "en"
    /// }
    /// ```
    /// Result: 20 liters = 5.28 gallons (US)
    /// 
    /// 5. **Chinese Locale Example**:
    /// ```json
    /// {
    ///   "category": "length",
    ///   "fromUnit": "m",
    ///   "toUnit": "ft",
    ///   "value": 100,
    ///   "locale": "zh"
    /// }
    /// ```
    /// 
    /// **Error Scenarios:**
    /// - Invalid category: Returns 404 with error message
    /// - Invalid unit: Returns 404 with error message
    /// - Units from different categories: Returns 400 with error message
    /// - Missing required fields: Returns 400 with error message
    /// </remarks>
    /// <response code="200">Conversion successful</response>
    /// <response code="400">Bad request (missing fields, invalid conversion, etc.)</response>
    /// <response code="404">Category or unit not found</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConvertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Convert units",
        Description = "Converts a value from one unit to another within the same category. Supports linear and formula-based conversions.",
        OperationId = "Convert",
        Tags = new[] { "Conversion" }
    )]
    [SwaggerResponse(200, "Conversion successful", typeof(ConvertResponseDto))]
    [SwaggerResponse(400, "Bad request - missing required fields or invalid conversion", typeof(object))]
    [SwaggerResponse(404, "Category or unit not found", typeof(object))]
    [SwaggerResponse(500, "Internal server error", typeof(object))]
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

