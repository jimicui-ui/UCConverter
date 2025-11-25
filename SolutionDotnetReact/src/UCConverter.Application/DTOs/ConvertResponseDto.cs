namespace UCConverter.Application.DTOs;

/// <summary>
/// Response DTO for unit conversion
/// </summary>
/// <example>
/// {
///   "result": 34.4488188976378,
///   "formattedResult": "34.45",
///   "precision": 2,
///   "formula": null,
///   "fromUnit": {
///     "symbol": "m",
///     "name": "meter",
///     "isBaseUnit": true,
///     "isSIUnit": true,
///     "unitSystem": "SI"
///   },
///   "toUnit": {
///     "symbol": "ft",
///     "name": "foot",
///     "isBaseUnit": false,
///     "isSIUnit": false,
///     "unitSystem": "Imperial"
///   }
/// }
/// </example>
public class ConvertResponseDto
{
    /// <summary>
    /// The conversion result as a numeric value (double precision).
    /// This is the exact calculated result before formatting.
    /// </summary>
    /// <example>34.4488188976378</example>
    public double Result { get; set; }

    /// <summary>
    /// The formatted result string with appropriate precision for display.
    /// This value is rounded based on the Precision property and is suitable for user display.
    /// </summary>
    /// <example>34.45</example>
    public string FormattedResult { get; set; } = string.Empty;

    /// <summary>
    /// The number of decimal places used in the formatted result.
    /// Typically 2-10 decimal places depending on the conversion type and result magnitude.
    /// </summary>
    /// <example>2</example>
    public int Precision { get; set; }

    /// <summary>
    /// Optional conversion formula used for non-linear conversions (e.g., temperature conversions). Null for linear conversions.
    /// </summary>
    /// <example>null</example>
    public string? Formula { get; set; }

    /// <summary>
    /// Detailed information about the source unit
    /// </summary>
    public UnitInfoDto FromUnit { get; set; } = null!;

    /// <summary>
    /// Detailed information about the target unit
    /// </summary>
    public UnitInfoDto ToUnit { get; set; } = null!;
}

/// <summary>
/// Information about a unit including its properties and classification
/// </summary>
public class UnitInfoDto
{
    /// <summary>
    /// The unit symbol (e.g., "m" for meter, "kg" for kilogram)
    /// </summary>
    /// <example>m</example>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The full name of the unit
    /// </summary>
    /// <example>meter</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// True if this unit is the base unit for its category
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
}
