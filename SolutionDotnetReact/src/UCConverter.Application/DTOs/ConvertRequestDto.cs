using System.ComponentModel.DataAnnotations;

namespace UCConverter.Application.DTOs;

/// <summary>
/// Request DTO for unit conversion
/// </summary>
/// <example>
/// {
///   "category": "length",
///   "fromUnit": "m",
///   "toUnit": "ft",
///   "value": 10.5,
///   "locale": "en"
/// }
/// </example>
public class ConvertRequestDto
{
    /// <summary>
    /// The category name (e.g., "length", "weight", "temperature", "volume", "area", "time", "speed")
    /// </summary>
    /// <example>length</example>
    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The source unit symbol (e.g., "m" for meter, "kg" for kilogram, "°C" for Celsius)
    /// </summary>
    /// <example>m</example>
    [Required(ErrorMessage = "FromUnit is required")]
    public string FromUnit { get; set; } = string.Empty;

    /// <summary>
    /// The target unit symbol (e.g., "ft" for foot, "lb" for pound, "°F" for Fahrenheit)
    /// </summary>
    /// <example>ft</example>
    [Required(ErrorMessage = "ToUnit is required")]
    public string ToUnit { get; set; } = string.Empty;

    /// <summary>
    /// The numeric value to convert
    /// </summary>
    /// <example>10.5</example>
    [Required(ErrorMessage = "Value is required")]
    public double Value { get; set; }

    /// <summary>
    /// Optional locale for localized error messages and unit display names. Supported values: "en", "zh", "en-US", "zh-CN". Defaults to "en" if not specified.
    /// </summary>
    /// <example>en</example>
    public string? Locale { get; set; }
}
