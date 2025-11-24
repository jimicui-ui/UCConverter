namespace UCConverter.Application.Services;

using Microsoft.Extensions.Localization;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Resources;

/// <summary>
/// Service for providing localized strings
/// </summary>
public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer<SharedResources> _localizer;

    public LocalizationService(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    public string GetString(string key, params object[] args)
    {
        var value = _localizer[key];
        return args.Length > 0 ? string.Format(value, args) : value;
    }

    public string GetCategoryDisplayName(string categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            return categoryName;
        }

        var key = $"Category_{categoryName}";
        var localized = _localizer[key];
        
        // If translation not found, return the key or a default
        if (localized.ResourceNotFound)
        {
            // Fallback: capitalize first letter
            return char.ToUpper(categoryName[0]) + categoryName.Substring(1);
        }
        
        return localized;
    }

    public string GetUnitDisplayName(string categoryName, string unitSymbol, string defaultName)
    {
        var key = $"Unit_{categoryName}_{unitSymbol}";
        var localized = _localizer[key];
        
        // If translation not found, return the default name
        if (localized.ResourceNotFound)
        {
            return defaultName;
        }
        
        return localized;
    }

    public string GetErrorMessage(string errorKey, params object[] args)
    {
        var key = $"Error_{errorKey}";
        var value = _localizer[key];
        
        if (value.ResourceNotFound)
        {
            // Fallback to English
            return GetDefaultErrorMessage(errorKey, args);
        }
        
        return args.Length > 0 ? string.Format(value, args) : value;
    }

    private static string GetDefaultErrorMessage(string errorKey, object[] args)
    {
        return errorKey switch
        {
            "CategoryNotFound" => $"Category '{args[0]}' not found",
            "UnitNotFound" => $"Unit '{args[0]}' not found",
            "InvalidConversion" => "Invalid conversion",
            "InvalidInput" => "Invalid input",
            _ => "An error occurred"
        };
    }
}

