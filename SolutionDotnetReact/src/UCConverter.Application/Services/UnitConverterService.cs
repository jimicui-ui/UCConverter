namespace UCConverter.Application.Services;

using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Mappings;
using UCConverter.Domain.Interfaces;

/// <summary>
/// Application service for unit conversion operations
/// </summary>
public class UnitConverterService : IUnitConverterService
{
    private readonly IConversionService _conversionService;
    private readonly IUnitRepository _unitRepository;
    private readonly ILocalizationService _localizationService;

    public UnitConverterService(
        IConversionService conversionService, 
        IUnitRepository unitRepository,
        ILocalizationService localizationService)
    {
        _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
        _unitRepository = unitRepository ?? throw new ArgumentNullException(nameof(unitRepository));
        _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
    }

    public async Task<ConvertResponseDto> ConvertAsync(ConvertRequestDto request)
    {
        var result = await _conversionService.ConvertAsync(
            request.Category,
            request.FromUnit,
            request.ToUnit,
            request.Value);

        return result.ToConvertResponseDto();
    }

    public async Task<IEnumerable<ConvertResponseDto>> ConvertBatchAsync(ConvertRequestDto request, IEnumerable<string> targetUnits)
    {
        var results = await _conversionService.ConvertBatchAsync(
            request.Category,
            request.FromUnit,
            targetUnits,
            request.Value);

        return results.Select(r => r.ToConvertResponseDto());
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitRepository.GetAllCategoriesAsync();
        return categories.Select(c => c.ToCategoryDto(_localizationService));
    }

    public async Task<IEnumerable<UnitDto>> GetUnitsByCategoryAsync(string categoryName)
    {
        var units = await _unitRepository.GetUnitsByCategoryAsync(categoryName);
        return units.Select(u => u.ToUnitDto(_localizationService, categoryName));
    }
}

