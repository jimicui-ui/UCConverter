namespace UCConverter.Domain.Exceptions;

/// <summary>
/// Exception thrown when conversion is invalid (e.g., cross-category conversion)
/// </summary>
public class InvalidConversionException : UnitConversionException
{
    public string FromUnit { get; }
    public string ToUnit { get; }
    public string? Category { get; }

    public InvalidConversionException(string fromUnit, string toUnit, string? category = null) 
        : base($"Cannot convert from '{fromUnit}' to '{toUnit}'. {(category != null ? $"Units must be in the same category: {category}" : "Units must be in the same category.")}")
    {
        FromUnit = fromUnit;
        ToUnit = toUnit;
        Category = category;
    }
}

