namespace UCConverter.Application.DTOs;

/// <summary>
/// Response DTO for unit conversion
/// </summary>
public class ConvertResponseDto
{
    public double Result { get; set; }
    public string FormattedResult { get; set; } = string.Empty;
    public int Precision { get; set; }
    public string? Formula { get; set; }
    public UnitInfoDto FromUnit { get; set; } = null!;
    public UnitInfoDto ToUnit { get; set; } = null!;
}

public class UnitInfoDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsBaseUnit { get; set; }
    public bool IsSIUnit { get; set; }
    public string UnitSystem { get; set; } = string.Empty;
}

