namespace UCConverter.Infrastructure.Repositories;

using System.Text.Json;
using Microsoft.Extensions.Logging;
using UCConverter.Domain.Entities;
using UCConverter.Domain.Exceptions;
using UCConverter.Domain.Interfaces;
using UCConverter.Infrastructure.Data;

/// <summary>
/// JSON-based implementation of IUnitRepository
/// Loads unit definitions from JSON files in UnitsSettings folder
/// </summary>
public class JsonUnitRepository : IUnitRepository
{
    private readonly string _unitsSettingsPath;
    private readonly ILogger<JsonUnitRepository> _logger;
    private readonly Dictionary<string, Category> _categoriesCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _lockObject = new();
    private bool _isInitialized = false;

    public JsonUnitRepository(string unitsSettingsPath, ILogger<JsonUnitRepository> logger)
    {
        _unitsSettingsPath = unitsSettingsPath ?? throw new ArgumentNullException(nameof(unitsSettingsPath));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Initializes the repository by loading all JSON files from UnitsSettings folder
    /// This should be called at application startup
    /// </summary>
    public void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        lock (_lockObject)
        {
            if (_isInitialized)
            {
                return;
            }

            try
            {
                if (!Directory.Exists(_unitsSettingsPath))
                {
                    _logger.LogWarning("UnitsSettings directory not found at: {Path}", _unitsSettingsPath);
                    return;
                }

                var jsonFiles = Directory.GetFiles(_unitsSettingsPath, "*.json");
                _logger.LogInformation("Loading {Count} unit configuration files from {Path}", jsonFiles.Length, _unitsSettingsPath);

                foreach (var jsonFile in jsonFiles)
                {
                    try
                    {
                        LoadCategoryFromFile(jsonFile);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to load unit configuration from file: {File}", jsonFile);
                        // Continue loading other files even if one fails (graceful degradation)
                    }
                }

                _isInitialized = true;
                _logger.LogInformation("Successfully loaded {Count} categories", _categoriesCache.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing JsonUnitRepository");
                throw;
            }
        }
    }

    private void LoadCategoryFromFile(string filePath)
    {
        var jsonContent = File.ReadAllText(filePath);
        var categoryJson = JsonSerializer.Deserialize<UnitCategoryJson>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (categoryJson == null)
        {
            _logger.LogWarning("Failed to deserialize JSON file: {File}", filePath);
            return;
        }

        if (categoryJson.BaseUnit == null)
        {
            _logger.LogWarning("BaseUnit is null in category file: {File}", filePath);
            return;
        }

        var category = new Category
        {
            Name = categoryJson.Category,
            DisplayName = categoryJson.CategoryDisplayName,
            BaseUnit = MapUnitJson(categoryJson.BaseUnit, categoryJson.Category),
            Units = categoryJson.Units?.Select(u => MapUnitJson(u, categoryJson.Category)).ToList() ?? new List<Unit>()
        };

        // Add base unit to units list if not already present
        if (!category.Units.Any(u => u.Symbol.Equals(category.BaseUnit.Symbol, StringComparison.OrdinalIgnoreCase)))
        {
            category.Units.Add(category.BaseUnit);
        }

        _categoriesCache[category.Name] = category;
        _logger.LogDebug("Loaded category: {Category} with {UnitCount} units", category.Name, category.Units.Count);
    }

    private static Unit MapUnitJson(UnitJson? unitJson, string category)
    {
        if (unitJson == null)
        {
            throw new ArgumentNullException(nameof(unitJson), "UnitJson cannot be null");
        }

        return new Unit
        {
            Symbol = unitJson.Symbol ?? string.Empty,
            Name = unitJson.Name ?? string.Empty,
            DisplayName = unitJson.DisplayName ?? string.Empty,
            Category = category,
            IsBaseUnit = unitJson.IsBaseUnit,
            IsSIUnit = unitJson.IsSIUnit,
            UnitSystem = unitJson.UnitSystem ?? string.Empty,
            ConversionFactor = unitJson.ConversionFactor,
            ConversionFormula = unitJson.ConversionFormula
        };
    }

    public Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        EnsureInitialized();
        return Task.FromResult<IEnumerable<Category>>(_categoriesCache.Values);
    }

    public Task<Category?> GetCategoryByNameAsync(string categoryName)
    {
        EnsureInitialized();
        _categoriesCache.TryGetValue(categoryName, out var category);
        return Task.FromResult(category);
    }

    public async Task<IEnumerable<Unit>> GetUnitsByCategoryAsync(string categoryName)
    {
        var category = await GetCategoryByNameAsync(categoryName);
        if (category == null)
        {
            return Enumerable.Empty<Unit>();
        }
        return category.Units;
    }

    public async Task<Unit?> GetUnitBySymbolAsync(string categoryName, string unitSymbol)
    {
        var category = await GetCategoryByNameAsync(categoryName);
        return category?.GetUnitBySymbol(unitSymbol);
    }

    public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
    {
        var categories = await GetAllCategoriesAsync();
        return categories.SelectMany(c => c.Units);
    }

    private void EnsureInitialized()
    {
        if (!_isInitialized)
        {
            Initialize();
        }
    }
}

