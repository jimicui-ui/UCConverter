namespace UCConverter.Domain.Entities;

/// <summary>
/// Represents a unit of measurement in the system
/// </summary>
public class Unit
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsBaseUnit { get; set; }
    public bool IsSIUnit { get; set; }
    public string UnitSystem { get; set; } = string.Empty;
    public double? ConversionFactor { get; set; }
    public string? ConversionFormula { get; set; }
    public string? ConversionInverseFormula { get; set; }

    /// <summary>
    /// Converts a value from this unit to the base unit of the category
    /// </summary>
    public double ConvertToBase(double value)
    {
        if (IsBaseUnit)
        {
            return value;
        }

        if (ConversionFactor.HasValue)
        {
            return value * ConversionFactor.Value;
        }

        if (!string.IsNullOrEmpty(ConversionFormula))
        {
            // For formula-based conversions (like temperature), 
            // this will be handled by the conversion service
            throw new InvalidOperationException($"Formula-based conversion not supported in Unit.ConvertToBase. Use conversion service for unit: {Symbol}");
        }

        throw new InvalidOperationException($"No conversion method available for unit: {Symbol}");
    }

    /// <summary>
    /// Converts a value from the base unit to this unit
    /// </summary>
    public double ConvertFromBase(double baseValue)
    {
        if (IsBaseUnit)
        {
            return baseValue;
        }

        if (ConversionFactor.HasValue && ConversionFactor.Value != 0)
        {
            return baseValue / ConversionFactor.Value;
        }

        if (!string.IsNullOrEmpty(ConversionFormula))
        {
            // For formula-based conversions, handled by conversion service
            throw new InvalidOperationException($"Formula-based conversion not supported in Unit.ConvertFromBase. Use conversion service for unit: {Symbol}");
        }

        throw new InvalidOperationException($"No conversion method available for unit: {Symbol}");
    }
}

