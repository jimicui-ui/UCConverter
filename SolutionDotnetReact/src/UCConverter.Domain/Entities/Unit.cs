namespace UCConverter.Domain.Entities;

using System.Text.RegularExpressions;

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
    /// Validates if a unit symbol contains only valid characters
    /// Allows letters, numbers, spaces, and common unit symbols (·, Ω, µ, °, superscripts, subscripts, /, parentheses)
    /// </summary>
    /// <param name="symbol">The unit symbol to validate</param>
    /// <returns>True if the symbol is valid, false otherwise</returns>
    public static bool IsValidUnitSymbol(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            return false;
        
        // Allow letters, numbers, spaces, and common unit symbols
        // Pattern allows: a-z, A-Z, 0-9, spaces, · (middle dot), Ω (omega), µ (micro), ° (degree)
        // Superscripts: ²³⁴⁵⁶⁷⁸⁹⁻¹²³⁴⁵⁶⁷⁸⁹⁰
        // Subscripts, /, parentheses, and other common unit characters
        var pattern = @"^[a-zA-Z0-9\s·Ωµ°²³⁴⁵⁶⁷⁸⁹⁻¹²³⁴⁵⁶⁷⁸⁹⁰\/\(\)\-\+×\*]+$";
        return Regex.IsMatch(symbol, pattern);
    }

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

