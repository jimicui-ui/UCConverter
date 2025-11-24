namespace UCConverter.Application.Mappings;

using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Entities;

/// <summary>
/// Mapping extensions for converting domain entities to DTOs
/// </summary>
public static class ConversionMapping
{
    public static UnitInfoDto ToUnitInfoDto(this Unit unit)
    {
        return new UnitInfoDto
        {
            Symbol = unit.Symbol,
            Name = unit.Name,
            IsBaseUnit = unit.IsBaseUnit,
            IsSIUnit = unit.IsSIUnit,
            UnitSystem = unit.UnitSystem
        };
    }

    public static ConvertResponseDto ToConvertResponseDto(this ConversionResult result)
    {
        return new ConvertResponseDto
        {
            Result = result.Result,
            FormattedResult = result.FormattedResult,
            Precision = result.Precision,
            Formula = result.Formula,
            FromUnit = result.FromUnit.ToUnitInfoDto(),
            ToUnit = result.ToUnit.ToUnitInfoDto()
        };
    }

    public static CategoryDto ToCategoryDto(this Category category, ILocalizationService? localizationService = null)
    {
        var displayName = localizationService != null
            ? localizationService.GetCategoryDisplayName(category.Name)
            : category.DisplayName;

        return new CategoryDto
        {
            Name = category.Name,
            DisplayName = displayName
        };
    }

    public static UnitDto ToUnitDto(this Unit unit, ILocalizationService? localizationService = null, string? categoryName = null)
    {
        var displayName = localizationService != null && !string.IsNullOrEmpty(categoryName)
            ? localizationService.GetUnitDisplayName(categoryName, unit.Symbol, unit.DisplayName)
            : unit.DisplayName;

        return new UnitDto
        {
            Symbol = unit.Symbol,
            Name = unit.Name,
            DisplayName = displayName,
            IsBaseUnit = unit.IsBaseUnit,
            IsSIUnit = unit.IsSIUnit,
            UnitSystem = unit.UnitSystem,
            ConversionFactor = unit.ConversionFactor
        };
    }
}

