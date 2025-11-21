namespace UCConverter.Application.Tests.Mappings;

using UCConverter.Application.DTOs;
using UCConverter.Application.Mappings;
using UCConverter.Domain.Entities;
using Xunit;

public class ConversionMappingTests
{
    [Fact]
    public void ToUnitInfoDto_WhenValidUnit_ReturnsDto()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI"
        };

        // Act
        var result = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("m", result.Symbol);
        Assert.Equal("meter", result.Name);
        Assert.True(result.IsBaseUnit);
        Assert.True(result.IsSIUnit);
        Assert.Equal("SI", result.UnitSystem);
    }

    [Fact]
    public void ToConvertResponseDto_WhenValidResult_ReturnsDto()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 1.0,
            FormattedResult = "1 km",
            Precision = 4,
            Formula = null,
            FromUnit = new Unit { Symbol = "m", Name = "meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI" },
            ToUnit = new Unit { Symbol = "km", Name = "kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI" },
            OriginalValue = 1000.0
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(1.0, dto.Result);
        Assert.Equal("1 km", dto.FormattedResult);
        Assert.Equal(4, dto.Precision);
        Assert.Null(dto.Formula);
        Assert.NotNull(dto.FromUnit);
        Assert.NotNull(dto.ToUnit);
    }

    [Fact]
    public void ToCategoryDto_WhenValidCategory_ReturnsDto()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length / Distance"
        };

        // Act
        var result = category.ToCategoryDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("length", result.Name);
        Assert.Equal("Length / Distance", result.DisplayName);
    }

    [Fact]
    public void ToUnitDto_WhenValidUnit_ReturnsDto()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };

        // Act
        var result = unit.ToUnitDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("m", result.Symbol);
        Assert.Equal("meter", result.Name);
        Assert.Equal("Meter", result.DisplayName);
        Assert.True(result.IsBaseUnit);
        Assert.True(result.IsSIUnit);
        Assert.Equal("SI", result.UnitSystem);
        Assert.Equal(1.0, result.ConversionFactor);
    }
}

