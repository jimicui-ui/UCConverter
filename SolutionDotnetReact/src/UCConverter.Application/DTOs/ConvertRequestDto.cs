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
    /// The category name (e.g., "length", "weight", "temperature", "volume", "area", "time", "speed").
    /// Must match an existing category name. Case-sensitive.
    /// </summary>
    /// <example>length</example>
    [Required(ErrorMessage = "Category is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 50 characters")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The source unit symbol (e.g., "m" for meter, "kg" for kilogram, "°C" for Celsius).
    /// Must be a valid unit symbol within the specified category. Case-sensitive.
    /// </summary>
    /// <example>m</example>
    [Required(ErrorMessage = "FromUnit is required")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "FromUnit must be between 1 and 20 characters")]
    public string FromUnit { get; set; } = string.Empty;

    /// <summary>
    /// The target unit symbol (e.g., "ft" for foot, "lb" for pound, "°F" for Fahrenheit).
    /// Must be a valid unit symbol within the specified category. Case-sensitive.
    /// </summary>
    /// <example>ft</example>
    [Required(ErrorMessage = "ToUnit is required")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "ToUnit must be between 1 and 20 characters")]
    public string ToUnit { get; set; } = string.Empty;

    /// <summary>
    /// The numeric value to convert. Must be a valid number (integer or decimal).
    /// Supports positive and negative values. Scientific notation is supported.
    /// </summary>
    /// <example>10.5</example>
    [Required(ErrorMessage = "Value is required")]
    public double Value { get; set; }

    /// <summary>
    /// Optional locale for localized error messages and unit display names.
    /// Supported values: "en", "zh", "en-US", "zh-CN".
    /// Defaults to "en" if not specified.
    /// Can also be set via Accept-Language header or query parameter.
    /// </summary>
    /// <example>en</example>
    [StringLength(10, ErrorMessage = "Locale must be 10 characters or less")]
    public string? Locale { get; set; }
}
