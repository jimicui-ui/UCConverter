namespace UCConverter.Domain.Entities;

/// <summary>
/// Represents the result of a unit conversion
/// </summary>
public class ConversionResult
{
    public double Result { get; set; }
    public string FormattedResult { get; set; } = string.Empty;
    public int Precision { get; set; } = 4;
    public string? Formula { get; set; }
    public Unit FromUnit { get; set; } = null!;
    public Unit ToUnit { get; set; } = null!;
    public double OriginalValue { get; set; }
}

