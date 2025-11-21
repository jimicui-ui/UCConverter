namespace UCConverter.Application.DTOs;

/// <summary>
/// DTO for unit information
/// </summary>
public class UnitDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsBaseUnit { get; set; }
    public bool IsSIUnit { get; set; }
    public string UnitSystem { get; set; } = string.Empty;
    public double? ConversionFactor { get; set; }
}

