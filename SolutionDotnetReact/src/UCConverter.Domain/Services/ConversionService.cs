namespace UCConverter.Domain.Services;

using System.Linq.Expressions;
using System.Text.RegularExpressions;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;

/// <summary>
/// Domain service for performing unit conversions
/// </summary>
public class ConversionService : IConversionService
{
    private readonly IUnitRepository _unitRepository;

    public ConversionService(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository ?? throw new ArgumentNullException(nameof(unitRepository));
    }

    public async Task<ConversionResult> ConvertAsync(string categoryName, string fromUnitSymbol, string toUnitSymbol, double value)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("Category name cannot be null or empty.", nameof(categoryName));
        if (string.IsNullOrWhiteSpace(fromUnitSymbol))
            throw new ArgumentException("From unit symbol cannot be null or empty.", nameof(fromUnitSymbol));
        if (string.IsNullOrWhiteSpace(toUnitSymbol))
            throw new ArgumentException("To unit symbol cannot be null or empty.", nameof(toUnitSymbol));

        // Get category
        var category = await _unitRepository.GetCategoryByNameAsync(categoryName);
        if (category == null)
        {
            throw new CategoryNotFoundException(categoryName);
        }

        // Get units
        var fromUnit = category.GetUnitBySymbol(fromUnitSymbol);
        var toUnit = category.GetUnitBySymbol(toUnitSymbol);

        if (fromUnit == null)
        {
            throw new UnitNotFoundException(fromUnitSymbol);
        }

        if (toUnit == null)
        {
            throw new UnitNotFoundException(toUnitSymbol);
        }

        // Validate units are in the same category
        if (fromUnit.Category != toUnit.Category)
        {
            throw new InvalidConversionException(fromUnitSymbol, toUnitSymbol, categoryName);
        }

        // Perform conversion
        double result;
        string? formula = null;

        // Check if either unit uses formula-based conversion (like temperature)
        if (!string.IsNullOrEmpty(fromUnit.ConversionFormula) || !string.IsNullOrEmpty(toUnit.ConversionFormula))
        {
            result = ConvertWithFormula(fromUnit, toUnit, value, out formula);
        }
        else
        {
            // Linear conversion: convert to base unit, then to target unit
            var baseValue = fromUnit.ConvertToBase(value);
            result = toUnit.ConvertFromBase(baseValue);
        }

        // Format result
        var formattedResult = $"{Math.Round(result, 4)} {toUnit.Symbol}";

        return new ConversionResult
        {
            Result = result,
            FormattedResult = formattedResult,
            Precision = 4,
            Formula = formula,
            FromUnit = fromUnit,
            ToUnit = toUnit,
            OriginalValue = value
        };
    }

    public async Task<IEnumerable<ConversionResult>> ConvertBatchAsync(string categoryName, string fromUnitSymbol, IEnumerable<string> toUnitSymbols, double value)
    {
        var results = new List<ConversionResult>();

        foreach (var toUnitSymbol in toUnitSymbols)
        {
            try
            {
                var result = await ConvertAsync(categoryName, fromUnitSymbol, toUnitSymbol, value);
                results.Add(result);
            }
            catch (Exception)
            {
                // Skip failed conversions in batch, but continue with others
                // In production, you might want to log these
                continue;
            }
        }

        return results;
    }

    private double ConvertWithFormula(Unit fromUnit, Unit toUnit, double value, out string? formula)
    {
        formula = null;

        // If both units have formulas, we need to convert through base unit (Kelvin for temperature)
        // Convert from source to base, then from base to target

        // Convert from source unit to base unit
        double baseValue;
        if (!string.IsNullOrEmpty(fromUnit.ConversionFormula))
        {
            baseValue = EvaluateFormula(fromUnit.ConversionFormula, value);
        }
        else
        {
            // Source unit uses conversion factor, convert to base
            baseValue = fromUnit.ConvertToBase(value);
        }

        // Convert from base unit to target unit
        double result;
        if (!string.IsNullOrEmpty(toUnit.ConversionFormula))
        {
            // Use inverse formula to convert from base to target
            if (string.IsNullOrEmpty(toUnit.ConversionInverseFormula))
            {
                throw new UnitConversionException($"Inverse formula is required for unit with formula: {toUnit.Symbol}");
            }
            result = EvaluateFormula(toUnit.ConversionInverseFormula, baseValue);
            formula = $"Converted via base unit using formula";
        }
        else
        {
            // Target unit uses conversion factor
            result = toUnit.ConvertFromBase(baseValue);
        }

        return result;
    }

    private double EvaluateFormula(string formula, double value)
    {
        // Simple formula evaluation for temperature conversions
        // Formula format: "x + 273.15" or "(x - 32) * 5/9 + 273.15" or "(x - 273.15) * 9/5 + 32"
        // Replace 'x' with the actual value
        var expression = formula.Replace("x", value.ToString(System.Globalization.CultureInfo.InvariantCulture));

        try
        {
            // Use DataTable.Compute for simple arithmetic expressions
            var dataTable = new System.Data.DataTable();
            var result = dataTable.Compute(expression, null);
            return Convert.ToDouble(result);
        }
        catch
        {
            throw new UnitConversionException($"Failed to evaluate conversion formula: {formula}");
        }
    }
}

