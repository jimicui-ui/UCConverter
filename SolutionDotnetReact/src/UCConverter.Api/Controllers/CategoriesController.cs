namespace UCConverter.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;

/// <summary>
/// Controller for category-related endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Category Management")]
public class CategoriesController : ControllerBase
{
    private readonly IUnitConverterService _unitConverterService;
    private readonly ILocalizationService _localizationService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(
        IUnitConverterService unitConverterService, 
        ILocalizationService localizationService,
        ILogger<CategoriesController> logger)
    {
        _unitConverterService = unitConverterService ?? throw new ArgumentNullException(nameof(unitConverterService));
        _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all available unit categories
    /// </summary>
    /// <param name="locale">Optional locale parameter for localized category names (e.g., "en", "zh", "en-US", "zh-CN"). Can be passed as query parameter or Accept-Language header.</param>
    /// <returns>List of all available unit categories</returns>
    /// <remarks>
    /// Returns all available unit categories in the system. Categories are returned with localized display names based on the locale parameter or Accept-Language header.
    /// 
    /// **Supported Categories:**
    /// 
    /// **Basic Categories:**
    /// - length: Length / Distance units (meter, foot, inch, etc.)
    /// - weight: Weight / Mass units (kilogram, pound, ounce, etc.)
    /// - temperature: Temperature units (Celsius, Fahrenheit, Kelvin)
    /// - volume: Volume units (liter, gallon, cubic meter, etc.)
    /// - area: Area units (square meter, square foot, acre, etc.)
    /// - time: Time units (second, minute, hour, etc.)
    /// - speed: Speed units (meter per second, mile per hour, etc.)
    /// 
    /// **Pressure &amp; Energy:**
    /// - pressure: Pressure units (pascal, bar, psi, atmosphere, etc.)
    /// - energy: Energy / Work units (joule, calorie, kilowatt-hour, BTU, etc.)
    /// - power: Power units (watt, horsepower, BTU/hour, etc.)
    /// 
    /// **Electricity Converters:**
    /// - charge: Electric charge (coulomb, ampere-hour, etc.)
    /// - linearChargeDensity: Linear charge density (coulomb per meter)
    /// - surfaceChargeDensity: Surface charge density (coulomb per square meter)
    /// - volumeChargeDensity: Volume charge density (coulomb per cubic meter)
    /// - current: Electric current (ampere, milliampere, etc.)
    /// - linearCurrentDensity: Linear current density (ampere per meter)
    /// - surfaceCurrentDensity: Surface current density (ampere per meter)
    /// - electricFieldStrength: Electric field strength (volt per meter)
    /// - electricPotential: Electric potential / Voltage (volt, kilovolt, etc.)
    /// - electricResistance: Electric resistance (ohm, kiloohm, etc.)
    /// - electricResistivity: Electric resistivity (ohm meter)
    /// - electricConductance: Electric conductance (siemens)
    /// - electricConductivity: Electric conductivity (siemens per meter)
    /// - capacitance: Electrostatic capacitance (farad, microfarad, etc.)
    /// - inductance: Inductance (henry, millihenry, etc.)
    /// 
    /// **Engineering Converters:**
    /// - angularVelocity: Angular velocity (radian per second, rpm, etc.)
    /// - acceleration: Acceleration (meter per second squared, g, etc.)
    /// - angularAcceleration: Angular acceleration (radian per second squared)
    /// - density: Density (kilogram per cubic meter, pound per cubic foot, etc.)
    /// - specificVolume: Specific volume (cubic meter per kilogram)
    /// - momentOfInertia: Moment of inertia (kilogram square meter)
    /// - momentOfForce: Moment of force (newton meter)
    /// - torque: Torque (newton meter, pound-force foot, etc.)
    /// 
    /// **Heat Converters:**
    /// - fuelEfficiencyMass: Fuel efficiency - mass (meter per kilogram, mile per pound)
    /// - fuelEfficiencyVolume: Fuel efficiency - volume (kilometer per liter, mpg, L/100km)
    /// - temperatureInterval: Temperature interval (kelvin, degree Celsius, degree Fahrenheit)
    /// - thermalExpansion: Thermal expansion coefficient (per kelvin)
    /// - thermalResistance: Thermal resistance (kelvin per watt)
    /// - thermalConductivity: Thermal conductivity (watt per meter kelvin)
    /// - specificHeatCapacity: Specific heat capacity (joule per kilogram kelvin)
    /// - heatDensity: Heat density (joule per cubic meter)
    /// - heatFluxDensity: Heat flux density (watt per square meter)
    /// - heatTransferCoefficient: Heat transfer coefficient (watt per square meter kelvin)
    /// 
    /// **Example Usage:**
    /// - Get categories in English: `GET /api/categories?locale=en`
    /// - Get categories in Chinese: `GET /api/categories?locale=zh`
    /// - Get categories using Accept-Language header: `GET /api/categories` with `Accept-Language: zh-CN`
    /// </remarks>
    /// <response code="200">Successfully retrieved list of categories</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Get all categories",
        Description = "Retrieves all available unit categories with localized display names",
        OperationId = "GetCategories",
        Tags = new[] { "Categories" }
    )]
    [SwaggerResponse(200, "Successfully retrieved categories", typeof(IEnumerable<CategoryDto>))]
    [SwaggerResponse(500, "Internal server error", typeof(object))]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] string? locale = null)
    {
        try
        {
            var categories = await _unitConverterService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories");
            var errorMessage = _localizationService.GetErrorMessage("InternalServerErrorCategories");
            return StatusCode(500, new { error = errorMessage });
        }
    }

    /// <summary>
    /// Get all units for a specific category
    /// </summary>
    /// <param name="name">The category name (e.g., "length", "weight", "temperature")</param>
    /// <param name="locale">Optional locale parameter for localized unit names (e.g., "en", "zh", "en-US", "zh-CN"). Can be passed as query parameter or Accept-Language header.</param>
    /// <returns>List of units in the specified category</returns>
    /// <remarks>
    /// Returns all units available in the specified category. Units are returned with localized display names based on the locale parameter or Accept-Language header.
    /// 
    /// **Category Examples:**
    /// - `length`: Returns units like meter (m), kilometer (km), foot (ft), inch (in), mile (mi)
    /// - `weight`: Returns units like kilogram (kg), gram (g), pound (lb), ounce (oz)
    /// - `temperature`: Returns units like Celsius (°C), Fahrenheit (°F), Kelvin (K)
    /// - `volume`: Returns units like liter (L), gallon (gal), cubic meter (m³)
    /// 
    /// **Example Usage:**
    /// - Get length units: `GET /api/categories/length/units`
    /// - Get weight units in Chinese: `GET /api/categories/weight/units?locale=zh`
    /// - Get temperature units: `GET /api/categories/temperature/units`
    /// </remarks>
    /// <response code="200">Successfully retrieved units for the category</response>
    /// <response code="404">Category not found</response>
    /// <response code="500">Internal server error occurred</response>
    [HttpGet("{name}/units")]
    [ProducesResponseType(typeof(IEnumerable<UnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Get units by category",
        Description = "Retrieves all units available in a specific category with localized display names",
        OperationId = "GetUnitsByCategory",
        Tags = new[] { "Categories" }
    )]
    [SwaggerResponse(200, "Successfully retrieved units", typeof(IEnumerable<UnitDto>))]
    [SwaggerResponse(404, "Category not found", typeof(object))]
    [SwaggerResponse(500, "Internal server error", typeof(object))]
    public async Task<ActionResult<IEnumerable<UnitDto>>> GetUnitsByCategory(
        [SwaggerParameter("The category name (e.g., 'length', 'weight', 'temperature')", Required = true)] string name,
        [FromQuery] string? locale = null)
    {
        try
        {
            var units = await _unitConverterService.GetUnitsByCategoryAsync(name);
            
            if (!units.Any())
            {
                var errorMessage = _localizationService.GetErrorMessage("CategoryNotFound", name);
                return NotFound(new { error = errorMessage, category = name });
            }

            return Ok(units);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving units for category: {Category}", name);
            var errorMessage = _localizationService.GetErrorMessage("InternalServerErrorUnits");
            return StatusCode(500, new { error = errorMessage });
        }
    }
}
