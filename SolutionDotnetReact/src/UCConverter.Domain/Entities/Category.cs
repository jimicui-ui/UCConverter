namespace UCConverter.Domain.Entities;

/// <summary>
/// Represents a unit category (e.g., Length, Weight, Temperature)
/// </summary>
public class Category
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public Unit BaseUnit { get; set; } = null!;
    public List<Unit> Units { get; set; } = new();

    /// <summary>
    /// Gets a unit by its symbol
    /// </summary>
    public Unit? GetUnitBySymbol(string symbol)
    {
        return Units.FirstOrDefault(u => u.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Validates that both units belong to this category
    /// </summary>
    public bool ValidateUnits(string fromUnitSymbol, string toUnitSymbol)
    {
        var fromUnit = GetUnitBySymbol(fromUnitSymbol);
        var toUnit = GetUnitBySymbol(toUnitSymbol);

        return fromUnit != null && toUnit != null;
    }
}

