namespace UCConverter.Application.Interfaces;

/// <summary>
/// Service for providing localized strings
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Gets a localized string for a key
    /// </summary>
    string GetString(string key, params object[] args);

    /// <summary>
    /// Gets a localized category display name
    /// </summary>
    string GetCategoryDisplayName(string categoryName);

    /// <summary>
    /// Gets a localized unit display name
    /// </summary>
    string GetUnitDisplayName(string categoryName, string unitSymbol, string defaultName);

    /// <summary>
    /// Gets a localized error message
    /// </summary>
    string GetErrorMessage(string errorKey, params object[] args);
}

