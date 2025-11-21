namespace UCConverter.Application.Interfaces;

using UCConverter.Application.DTOs;

/// <summary>
/// Application service interface for unit conversion operations
/// </summary>
public interface IUnitConverterService
{
    Task<ConvertResponseDto> ConvertAsync(ConvertRequestDto request);
    Task<IEnumerable<ConvertResponseDto>> ConvertBatchAsync(ConvertRequestDto request, IEnumerable<string> targetUnits);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<IEnumerable<UnitDto>> GetUnitsByCategoryAsync(string categoryName);
}

