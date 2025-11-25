namespace UCConverter.Application.DTOs;

/// <summary>
/// DTO for unit information
/// </summary>
/// <example>
/// {
///   "symbol": "m",
///   "name": "meter",
///   "displayName": "Meter",
///   "isBaseUnit": true,
///   "isSIUnit": true,
///   "unitSystem": "SI",
///   "conversionFactor": 1.0
/// }
/// </example>
public class UnitDto
{
    /// <summary>
    /// The unit symbol used in conversions (e.g., "m" for meter, "kg" for kilogram, "°C" for Celsius)
    /// </summary>
    /// <example>m</example>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The full name of the unit
    /// </summary>
    /// <example>meter</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The human-readable display name for the unit, localized based on the request locale
    /// </summary>
    /// <example>Meter</example>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// True if this unit is the base unit for its category (all conversions go through the base unit)
    /// </summary>
    /// <example>true</example>
    public bool IsBaseUnit { get; set; }

    /// <summary>
    /// True if this unit is part of the SI (International System of Units)
    /// </summary>
    /// <example>true</example>
    public bool IsSIUnit { get; set; }

    /// <summary>
    /// The unit system classification (e.g., "SI", "Imperial", "US Customary")
    /// </summary>
    /// <example>SI</example>
    public string UnitSystem { get; set; } = string.Empty;

    /// <summary>
    /// The conversion factor to convert this unit to the base unit. Null for formula-based conversions (e.g., temperature).
    /// </summary>
    /// <example>1.0</example>
    public double? ConversionFactor { get; set; }
}
