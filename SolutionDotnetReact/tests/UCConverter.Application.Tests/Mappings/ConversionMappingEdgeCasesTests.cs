namespace UCConverter.Application.Tests.Mappings;

using UCConverter.Application.DTOs;
using UCConverter.Application.Mappings;
using UCConverter.Domain.Entities;
using Xunit;

public class ConversionMappingEdgeCasesTests
{
    [Fact]
    public void ToUnitInfoDto_WhenUnitHasEmptyStrings_ReturnsDto()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "",
            Name = "",
            UnitSystem = "",
            IsBaseUnit = false,
            IsSIUnit = false
        };

        // Act
        var dto = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("", dto.Symbol);
        Assert.Equal("", dto.Name);
        Assert.Equal("", dto.UnitSystem);
        Assert.False(dto.IsBaseUnit);
        Assert.False(dto.IsSIUnit);
    }

    [Fact]
    public void ToConvertResponseDto_WhenFormulaIsNull_ReturnsDtoWithNullFormula()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 1.0,
            FormattedResult = "1 km",
            Precision = 4,
            Formula = null,
            FromUnit = new Unit { Symbol = "m", Name = "meter" },
            ToUnit = new Unit { Symbol = "km", Name = "kilometer" },
            OriginalValue = 1000.0
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Null(dto.Formula);
    }

    [Fact]
    public void ToConvertResponseDto_WhenFormulaIsNotNull_ReturnsDtoWithFormula()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 298.15,
            FormattedResult = "298.15 K",
            Precision = 4,
            Formula = "x + 273.15",
            FromUnit = new Unit { Symbol = "°C", Name = "celsius" },
            ToUnit = new Unit { Symbol = "K", Name = "kelvin" },
            OriginalValue = 25.0
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.NotNull(dto.Formula);
        Assert.Equal("x + 273.15", dto.Formula);
    }

    [Fact]
    public void ToCategoryDto_WhenCategoryHasEmptyStrings_ReturnsDto()
    {
        // Arrange
        var category = new Category
        {
            Name = "",
            DisplayName = ""
        };

        // Act
        var dto = category.ToCategoryDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("", dto.Name);
        Assert.Equal("", dto.DisplayName);
    }

    [Fact]
    public void ToUnitDto_WhenUnitHasNullConversionFactor_ReturnsDtoWithNull()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "test",
            Name = "test",
            DisplayName = "Test",
            IsBaseUnit = false,
            IsSIUnit = false,
            UnitSystem = "Custom",
            ConversionFactor = null
        };

        // Act
        var dto = unit.ToUnitDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Null(dto.ConversionFactor);
    }

    [Fact]
    public void ToUnitDto_WhenUnitHasConversionFactor_ReturnsDtoWithFactor()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            DisplayName = "Kilometer",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1000.0
        };

        // Act
        var dto = unit.ToUnitDto();

        // Assert
        Assert.NotNull(dto);
        Assert.NotNull(dto.ConversionFactor);
        Assert.Equal(1000.0, dto.ConversionFactor);
    }
}

