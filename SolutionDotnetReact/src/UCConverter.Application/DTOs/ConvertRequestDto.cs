namespace UCConverter.Application.DTOs;

/// <summary>
/// Request DTO for unit conversion
/// </summary>
public class ConvertRequestDto
{
    public string Category { get; set; } = string.Empty;
    public string FromUnit { get; set; } = string.Empty;
    public string ToUnit { get; set; } = string.Empty;
    public double Value { get; set; }
    public string? Locale { get; set; }
}

