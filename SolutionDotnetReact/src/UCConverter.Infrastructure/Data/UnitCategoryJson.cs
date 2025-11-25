namespace UCConverter.Infrastructure.Data;

/// <summary>
/// JSON model for deserializing unit category files
/// </summary>
public class UnitCategoryJson
{
    public string Category { get; set; } = string.Empty;
    public string CategoryDisplayName { get; set; } = string.Empty;
    public UnitJson BaseUnit { get; set; } = null!;
    public List<UnitJson> Units { get; set; } = new();
}

public class UnitJson
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
}

