namespace UCConverter.Application.DTOs;

/// <summary>
/// DTO for category information
/// </summary>
/// <example>
/// {
///   "name": "length",
///   "displayName": "Length / Distance"
/// }
/// </example>
public class CategoryDto
{
    /// <summary>
    /// The category identifier name (e.g., "length", "weight", "temperature", "volume", "area", "time", "speed").
    /// This is the key used in API requests. Must match exactly (case-sensitive).
    /// </summary>
    /// <example>length</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The human-readable display name for the category, localized based on the request locale.
    /// This value changes based on the locale parameter or Accept-Language header.
    /// </summary>
    /// <example>Length / Distance</example>
    public string DisplayName { get; set; } = string.Empty;
}
