namespace UCConverter.Domain.Interfaces;

using UCConverter.Domain.Entities;

/// <summary>
/// Service interface for performing unit conversions
/// </summary>
public interface IConversionService
{
    /// <summary>
    /// Converts a value from one unit to another within the same category
    /// </summary>
    /// <param name="categoryName">The category name</param>
    /// <param name="fromUnitSymbol">Source unit symbol</param>
    /// <param name="toUnitSymbol">Target unit symbol</param>
    /// <param name="value">Value to convert</param>
    /// <returns>Conversion result</returns>
    Task<ConversionResult> ConvertAsync(string categoryName, string fromUnitSymbol, string toUnitSymbol, double value);

    /// <summary>
    /// Converts a value to multiple target units (batch conversion)
    /// </summary>
    /// <param name="categoryName">The category name</param>
    /// <param name="fromUnitSymbol">Source unit symbol</param>
    /// <param name="toUnitSymbols">Target unit symbols</param>
    /// <param name="value">Value to convert</param>
    /// <returns>List of conversion results</returns>
    Task<IEnumerable<ConversionResult>> ConvertBatchAsync(string categoryName, string fromUnitSymbol, IEnumerable<string> toUnitSymbols, double value);
}

