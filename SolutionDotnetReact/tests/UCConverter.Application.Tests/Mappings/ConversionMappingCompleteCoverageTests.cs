namespace UCConverter.Application.Tests.Mappings;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Mappings;
using UCConverter.Domain.Entities;
using Xunit;

/// <summary>
/// Comprehensive tests to achieve 100% code coverage for ConversionMapping
/// </summary>
public class ConversionMappingCompleteCoverageTests
{
    #region ToUnitInfoDto Tests

    [Fact]
    public void ToUnitInfoDto_WhenUnitIsBaseUnit_MapsCorrectly()
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
    public void ToUnitInfoDto_WhenUnitIsNonBaseUnit_MapsCorrectly()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI"
        };

        // Act
        var result = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("km", result.Symbol);
        Assert.False(result.IsBaseUnit);
    }

    [Fact]
    public void ToUnitInfoDto_WhenUnitIsNonSI_MapsCorrectly()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "ft",
            Name = "foot",
            IsBaseUnit = false,
            IsSIUnit = false,
            UnitSystem = "Imperial"
        };

        // Act
        var result = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSIUnit);
        Assert.Equal("Imperial", result.UnitSystem);
    }

    #endregion

    #region ToConvertResponseDto Tests

    [Fact]
    public void ToConvertResponseDto_WhenResultHasFormula_MapsFormula()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 25.0,
            FormattedResult = "25 °C",
            Precision = 2,
            Formula = "x - 273.15",
            FromUnit = new Unit { Symbol = "K", Name = "kelvin" },
            ToUnit = new Unit { Symbol = "°C", Name = "celsius" }
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(25.0, dto.Result);
        Assert.Equal("25 °C", dto.FormattedResult);
        Assert.Equal(2, dto.Precision);
        Assert.Equal("x - 273.15", dto.Formula);
        Assert.NotNull(dto.FromUnit);
        Assert.NotNull(dto.ToUnit);
    }

    [Fact]
    public void ToConvertResponseDto_WhenResultHasNullFormula_MapsNullFormula()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 1000.0,
            FormattedResult = "1000 m",
            Precision = 2,
            Formula = null,
            FromUnit = new Unit { Symbol = "km", Name = "kilometer" },
            ToUnit = new Unit { Symbol = "m", Name = "meter" }
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(1000.0, dto.Result);
        Assert.Null(dto.Formula);
    }

    [Fact]
    public void ToConvertResponseDto_WhenResultHasEmptyFormula_MapsEmptyFormula()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 100.0,
            FormattedResult = "100 m",
            Precision = 2,
            Formula = "",
            FromUnit = new Unit { Symbol = "m", Name = "meter" },
            ToUnit = new Unit { Symbol = "m", Name = "meter" }
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("", dto.Formula);
    }

    [Fact]
    public void ToConvertResponseDto_WhenResultHasZeroPrecision_MapsPrecision()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 100.0,
            FormattedResult = "100 m",
            Precision = 0,
            Formula = null,
            FromUnit = new Unit { Symbol = "m", Name = "meter" },
            ToUnit = new Unit { Symbol = "m", Name = "meter" }
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(0, dto.Precision);
    }

    [Fact]
    public void ToConvertResponseDto_WhenResultHasNegativeValue_MapsCorrectly()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = -10.0,
            FormattedResult = "-10 m",
            Precision = 2,
            Formula = null,
            FromUnit = new Unit { Symbol = "m", Name = "meter" },
            ToUnit = new Unit { Symbol = "m", Name = "meter" }
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(-10.0, dto.Result);
    }

    #endregion

    #region ToCategoryDto Tests

    [Fact]
    public void ToCategoryDto_WhenLocalizationServiceIsNull_UsesCategoryDisplayName()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common"
        };

        // Act
        var result = category.ToCategoryDto(null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("length", result.Name);
        Assert.Equal("Length", result.DisplayName);
        Assert.Equal("Common", result.Group);
    }

    [Fact]
    public void ToCategoryDto_WhenLocalizationServiceProvided_UsesLocalizedDisplayName()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common"
        };

        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(s => s.GetCategoryDisplayName("length")).Returns("Length / Distance");

        // Act
        var result = category.ToCategoryDto(mockLocalizationService.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("length", result.Name);
        Assert.Equal("Length / Distance", result.DisplayName);
        Assert.Equal("Common", result.Group);
    }

    [Fact]
    public void ToCategoryDto_WhenGroupIsEngineering_MapsGroup()
    {
        // Arrange
        var category = new Category
        {
            Name = "acceleration",
            DisplayName = "Acceleration",
            Group = "Engineering"
        };

        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(s => s.GetCategoryDisplayName("acceleration")).Returns("Acceleration");

        // Act
        var result = category.ToCategoryDto(mockLocalizationService.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Engineering", result.Group);
    }

    [Fact]
    public void ToCategoryDto_WhenGroupIsElectricity_MapsGroup()
    {
        // Arrange
        var category = new Category
        {
            Name = "current",
            DisplayName = "Current",
            Group = "Electricity"
        };

        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(s => s.GetCategoryDisplayName("current")).Returns("Current");

        // Act
        var result = category.ToCategoryDto(mockLocalizationService.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Electricity", result.Group);
    }

    [Fact]
    public void ToCategoryDto_WhenGroupIsHeat_MapsGroup()
    {
        // Arrange
        var category = new Category
        {
            Name = "thermalConductivity",
            DisplayName = "Thermal Conductivity",
            Group = "Heat"
        };

        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(s => s.GetCategoryDisplayName("thermalConductivity")).Returns("Thermal Conductivity");

        // Act
        var result = category.ToCategoryDto(mockLocalizationService.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Heat", result.Group);
    }

    #endregion

    #region ToUnitDto Tests

    [Fact]
    public void ToUnitDto_WhenLocalizationServiceIsNull_UsesUnitDisplayName()
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
        var result = unit.ToUnitDto(null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("m", result.Symbol);
        Assert.Equal("Meter", result.DisplayName);
    }

    [Fact]
    public void ToUnitDto_WhenLocalizationServiceProvidedAndCategoryNameProvided_UsesLocalizedDisplayName()
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

        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(s => s.GetUnitDisplayName("length", "m", "Meter")).Returns("Meter (Localized)");

        // Act
        var result = unit.ToUnitDto(mockLocalizationService.Object, "length");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meter (Localized)", result.DisplayName);
        mockLocalizationService.Verify(s => s.GetUnitDisplayName("length", "m", "Meter"), Times.Once);
    }

    [Fact]
    public void ToUnitDto_WhenLocalizationServiceProvidedButCategoryNameIsNull_UsesUnitDisplayName()
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

        var mockLocalizationService = new Mock<ILocalizationService>();

        // Act
        var result = unit.ToUnitDto(mockLocalizationService.Object, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meter", result.DisplayName);
        mockLocalizationService.Verify(s => s.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ToUnitDto_WhenLocalizationServiceProvidedButCategoryNameIsEmpty_UsesUnitDisplayName()
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

        var mockLocalizationService = new Mock<ILocalizationService>();

        // Act
        var result = unit.ToUnitDto(mockLocalizationService.Object, "");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meter", result.DisplayName);
        mockLocalizationService.Verify(s => s.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ToUnitDto_WhenLocalizationServiceProvidedButCategoryNameIsWhitespace_UsesLocalizedDisplayName()
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

        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(s => s.GetUnitDisplayName("   ", "m", "Meter")).Returns("Meter (Localized)");

        // Act
        var result = unit.ToUnitDto(mockLocalizationService.Object, "   ");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meter (Localized)", result.DisplayName);
        mockLocalizationService.Verify(s => s.GetUnitDisplayName("   ", "m", "Meter"), Times.Once);
    }

    [Fact]
    public void ToUnitDto_WhenUnitHasNullConversionFactor_MapsNull()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            DisplayName = "Celsius",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = null
        };

        // Act
        var result = unit.ToUnitDto(null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ConversionFactor);
    }

    [Fact]
    public void ToUnitDto_WhenUnitHasZeroConversionFactor_MapsZero()
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
            ConversionFactor = 0.0
        };

        // Act
        var result = unit.ToUnitDto(null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.ConversionFactor);
    }

    [Fact]
    public void ToUnitDto_WhenUnitHasLargeConversionFactor_MapsCorrectly()
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
        var result = unit.ToUnitDto(null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1000.0, result.ConversionFactor);
    }

    [Fact]
    public void ToUnitDto_WhenUnitHasSmallConversionFactor_MapsCorrectly()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "mm",
            Name = "millimeter",
            DisplayName = "Millimeter",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 0.001
        };

        // Act
        var result = unit.ToUnitDto(null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.001, result.ConversionFactor);
    }

    [Fact]
    public void ToUnitDto_WhenAllPropertiesSet_MapsAllProperties()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "kg",
            Name = "kilogram",
            DisplayName = "Kilogram",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };

        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(s => s.GetUnitDisplayName("weight", "kg", "Kilogram")).Returns("Kilogram (Localized)");

        // Act
        var result = unit.ToUnitDto(mockLocalizationService.Object, "weight");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("kg", result.Symbol);
        Assert.Equal("kilogram", result.Name);
        Assert.Equal("Kilogram (Localized)", result.DisplayName);
        Assert.True(result.IsBaseUnit);
        Assert.True(result.IsSIUnit);
        Assert.Equal("SI", result.UnitSystem);
        Assert.Equal(1.0, result.ConversionFactor);
    }

    #endregion
}

