namespace UCConverter.Domain.Interfaces;

using UCConverter.Domain.Entities;

/// <summary>
/// Repository interface for accessing unit and category data
/// </summary>
public interface IUnitRepository
{
    /// <summary>
    /// Gets all available categories
    /// </summary>
    Task<IEnumerable<Category>> GetAllCategoriesAsync();

    /// <summary>
    /// Gets a category by name
    /// </summary>
    Task<Category?> GetCategoryByNameAsync(string categoryName);

    /// <summary>
    /// Gets all units for a specific category
    /// </summary>
    Task<IEnumerable<Unit>> GetUnitsByCategoryAsync(string categoryName);

    /// <summary>
    /// Gets a unit by symbol and category
    /// </summary>
    Task<Unit?> GetUnitBySymbolAsync(string categoryName, string unitSymbol);

    /// <summary>
    /// Gets all units (across all categories)
    /// </summary>
    Task<IEnumerable<Unit>> GetAllUnitsAsync();
}

