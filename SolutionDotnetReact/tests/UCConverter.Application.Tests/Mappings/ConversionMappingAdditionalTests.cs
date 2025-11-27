namespace UCConverter.Application.Tests.Mappings;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Mappings;
using UCConverter.Domain.Entities;
using Xunit;

/// <summary>
/// Additional mapping tests to improve coverage
/// </summary>
public class ConversionMappingAdditionalTests
{
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
            ToUnit = new Unit { Symbol = "km", Name = "kilometer" }
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Null(dto.Formula);
        Assert.Equal(1.0, dto.Result);
    }

    [Fact]
    public void ToConvertResponseDto_WhenFormulaIsProvided_ReturnsDtoWithFormula()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 77.0,
            FormattedResult = "77 °F",
            Precision = 4,
            Formula = "x * 9/5 + 32",
            FromUnit = new Unit { Symbol = "°C", Name = "celsius" },
            ToUnit = new Unit { Symbol = "°F", Name = "fahrenheit" }
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("x * 9/5 + 32", dto.Formula);
    }

    [Fact]
    public void ToCategoryDto_WhenGroupIsEngineering_ReturnsDtoWithEngineeringGroup()
    {
        // Arrange
        var category = new Category
        {
            Name = "acceleration",
            DisplayName = "Acceleration",
            Group = "Engineering",
            BaseUnit = new Unit { Symbol = "m/s²", Category = "acceleration" },
            Units = new List<Unit>()
        };

        // Act
        var dto = category.ToCategoryDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("Engineering", dto.Group);
    }

    [Fact]
    public void ToCategoryDto_WhenGroupIsElectricity_ReturnsDtoWithElectricityGroup()
    {
        // Arrange
        var category = new Category
        {
            Name = "current",
            DisplayName = "Current",
            Group = "Electricity",
            BaseUnit = new Unit { Symbol = "A", Category = "current" },
            Units = new List<Unit>()
        };

        // Act
        var dto = category.ToCategoryDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("Electricity", dto.Group);
    }

    [Fact]
    public void ToCategoryDto_WhenGroupIsHeat_ReturnsDtoWithHeatGroup()
    {
        // Arrange
        var category = new Category
        {
            Name = "thermalConductivity",
            DisplayName = "Thermal Conductivity",
            Group = "Heat",
            BaseUnit = new Unit { Symbol = "W/(m·K)", Category = "thermalConductivity" },
            Units = new List<Unit>()
        };

        // Act
        var dto = category.ToCategoryDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("Heat", dto.Group);
    }

    [Fact]
    public void ToUnitDto_WhenAllPropertiesSet_MapsAllProperties()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            Category = "length",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0,
            ConversionFormula = null,
            ConversionInverseFormula = null
        };

        // Act
        var dto = unit.ToUnitDto(null, "length");

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("m", dto.Symbol);
        Assert.Equal("meter", dto.Name);
        Assert.Equal("Meter", dto.DisplayName);
        Assert.True(dto.IsBaseUnit);
        Assert.True(dto.IsSIUnit);
        Assert.Equal("SI", dto.UnitSystem);
        Assert.Equal(1.0, dto.ConversionFactor);
    }

    [Fact]
    public void ToUnitInfoDto_WhenAllPropertiesSet_MapsAllProperties()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            Category = "length",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };

        // Act
        var dto = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("m", dto.Symbol);
        Assert.Equal("meter", dto.Name);
        Assert.True(dto.IsBaseUnit);
        Assert.True(dto.IsSIUnit);
        Assert.Equal("SI", dto.UnitSystem);
    }

    [Fact]
    public void ToUnitInfoDto_WhenUnitIsNotBaseUnit_MapsCorrectly()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            DisplayName = "Kilometer",
            Category = "length",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1000.0
        };

        // Act
        var dto = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(dto);
        Assert.False(dto.IsBaseUnit);
        Assert.True(dto.IsSIUnit);
    }

    [Fact]
    public void ToUnitInfoDto_WhenUnitIsNotSIUnit_MapsCorrectly()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "ft",
            Name = "foot",
            DisplayName = "Foot",
            Category = "length",
            IsBaseUnit = false,
            IsSIUnit = false,
            UnitSystem = "Imperial",
            ConversionFactor = 0.3048
        };

        // Act
        var dto = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(dto);
        Assert.False(dto.IsSIUnit);
        Assert.Equal("Imperial", dto.UnitSystem);
    }
}
