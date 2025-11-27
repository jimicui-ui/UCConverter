namespace UCConverter.Application.Tests.DTOs;

using UCConverter.Application.DTOs;
using Xunit;

/// <summary>
/// Tests to ensure DTOs are instantiated and used, contributing to code coverage
/// </summary>
public class DTOsCoverageTests
{
    #region ConvertRequestDto Tests

    [Fact]
    public void ConvertRequestDto_CanBeInstantiated()
    {
        // Act
        var dto = new ConvertRequestDto();

        // Assert
        Assert.NotNull(dto);
    }

    [Fact]
    public void ConvertRequestDto_CanSetAllProperties()
    {
        // Arrange
        var dto = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0,
            Locale = "en"
        };

        // Assert
        Assert.Equal("length", dto.Category);
        Assert.Equal("m", dto.FromUnit);
        Assert.Equal("km", dto.ToUnit);
        Assert.Equal(1000.0, dto.Value);
        Assert.Equal("en", dto.Locale);
    }

    [Fact]
    public void ConvertRequestDto_CanSetLocaleToNull()
    {
        // Arrange
        var dto = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0,
            Locale = null
        };

        // Assert
        Assert.Null(dto.Locale);
    }

    [Fact]
    public void ConvertRequestDto_CanSetNegativeValue()
    {
        // Arrange
        var dto = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°C",
            ToUnit = "°F",
            Value = -10.0
        };

        // Assert
        Assert.Equal(-10.0, dto.Value);
    }

    [Fact]
    public void ConvertRequestDto_CanSetZeroValue()
    {
        // Arrange
        var dto = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 0.0
        };

        // Assert
        Assert.Equal(0.0, dto.Value);
    }

    #endregion

    #region ConvertResponseDto Tests

    [Fact]
    public void ConvertResponseDto_CanBeInstantiated()
    {
        // Act
        var dto = new ConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
    }

    [Fact]
    public void ConvertResponseDto_CanSetAllProperties()
    {
        // Arrange
        var dto = new ConvertResponseDto
        {
            Result = 1000.0,
            FormattedResult = "1000 m",
            Precision = 2,
            Formula = "x / 1000",
            FromUnit = new UnitInfoDto { Symbol = "km", Name = "kilometer" },
            ToUnit = new UnitInfoDto { Symbol = "m", Name = "meter" }
        };

        // Assert
        Assert.Equal(1000.0, dto.Result);
        Assert.Equal("1000 m", dto.FormattedResult);
        Assert.Equal(2, dto.Precision);
        Assert.Equal("x / 1000", dto.Formula);
        Assert.NotNull(dto.FromUnit);
        Assert.NotNull(dto.ToUnit);
    }

    [Fact]
    public void ConvertResponseDto_CanSetFormulaToNull()
    {
        // Arrange
        var dto = new ConvertResponseDto
        {
            Result = 1000.0,
            FormattedResult = "1000 m",
            Precision = 2,
            Formula = null,
            FromUnit = new UnitInfoDto { Symbol = "km", Name = "kilometer" },
            ToUnit = new UnitInfoDto { Symbol = "m", Name = "meter" }
        };

        // Assert
        Assert.Null(dto.Formula);
    }

    [Fact]
    public void ConvertResponseDto_CanSetNegativeResult()
    {
        // Arrange
        var dto = new ConvertResponseDto
        {
            Result = -10.0,
            FormattedResult = "-10 °C",
            Precision = 2,
            Formula = null,
            FromUnit = new UnitInfoDto { Symbol = "°F", Name = "fahrenheit" },
            ToUnit = new UnitInfoDto { Symbol = "°C", Name = "celsius" }
        };

        // Assert
        Assert.Equal(-10.0, dto.Result);
    }

    [Fact]
    public void ConvertResponseDto_CanSetZeroPrecision()
    {
        // Arrange
        var dto = new ConvertResponseDto
        {
            Result = 100.0,
            FormattedResult = "100 m",
            Precision = 0,
            Formula = null,
            FromUnit = new UnitInfoDto { Symbol = "m", Name = "meter" },
            ToUnit = new UnitInfoDto { Symbol = "m", Name = "meter" }
        };

        // Assert
        Assert.Equal(0, dto.Precision);
    }

    #endregion

    #region CategoryDto Tests

    [Fact]
    public void CategoryDto_CanBeInstantiated()
    {
        // Act
        var dto = new CategoryDto();

        // Assert
        Assert.NotNull(dto);
    }

    [Fact]
    public void CategoryDto_CanSetAllProperties()
    {
        // Arrange
        var dto = new CategoryDto
        {
            Name = "length",
            DisplayName = "Length / Distance",
            Group = "Common"
        };

        // Assert
        Assert.Equal("length", dto.Name);
        Assert.Equal("Length / Distance", dto.DisplayName);
        Assert.Equal("Common", dto.Group);
    }

    [Fact]
    public void CategoryDto_CanSetGroupToEngineering()
    {
        // Arrange
        var dto = new CategoryDto
        {
            Name = "acceleration",
            DisplayName = "Acceleration",
            Group = "Engineering"
        };

        // Assert
        Assert.Equal("Engineering", dto.Group);
    }

    [Fact]
    public void CategoryDto_CanSetGroupToElectricity()
    {
        // Arrange
        var dto = new CategoryDto
        {
            Name = "current",
            DisplayName = "Current",
            Group = "Electricity"
        };

        // Assert
        Assert.Equal("Electricity", dto.Group);
    }

    [Fact]
    public void CategoryDto_CanSetGroupToHeat()
    {
        // Arrange
        var dto = new CategoryDto
        {
            Name = "thermalConductivity",
            DisplayName = "Thermal Conductivity",
            Group = "Heat"
        };

        // Assert
        Assert.Equal("Heat", dto.Group);
    }

    #endregion

    #region UnitDto Tests

    [Fact]
    public void UnitDto_CanBeInstantiated()
    {
        // Act
        var dto = new UnitDto();

        // Assert
        Assert.NotNull(dto);
    }

    [Fact]
    public void UnitDto_CanSetAllProperties()
    {
        // Arrange
        var dto = new UnitDto
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };

        // Assert
        Assert.Equal("m", dto.Symbol);
        Assert.Equal("meter", dto.Name);
        Assert.Equal("Meter", dto.DisplayName);
        Assert.True(dto.IsBaseUnit);
        Assert.True(dto.IsSIUnit);
        Assert.Equal("SI", dto.UnitSystem);
        Assert.Equal(1.0, dto.ConversionFactor);
    }

    [Fact]
    public void UnitDto_CanSetConversionFactorToNull()
    {
        // Arrange
        var dto = new UnitDto
        {
            Symbol = "°C",
            Name = "celsius",
            DisplayName = "Celsius",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = null
        };

        // Assert
        Assert.Null(dto.ConversionFactor);
    }

    [Fact]
    public void UnitDto_CanSetIsBaseUnitToFalse()
    {
        // Arrange
        var dto = new UnitDto
        {
            Symbol = "km",
            Name = "kilometer",
            DisplayName = "Kilometer",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1000.0
        };

        // Assert
        Assert.False(dto.IsBaseUnit);
    }

    [Fact]
    public void UnitDto_CanSetIsSIUnitToFalse()
    {
        // Arrange
        var dto = new UnitDto
        {
            Symbol = "ft",
            Name = "foot",
            DisplayName = "Foot",
            IsBaseUnit = false,
            IsSIUnit = false,
            UnitSystem = "Imperial",
            ConversionFactor = 0.3048
        };

        // Assert
        Assert.False(dto.IsSIUnit);
        Assert.Equal("Imperial", dto.UnitSystem);
    }

    #endregion

    #region UnitInfoDto Tests

    [Fact]
    public void UnitInfoDto_CanBeInstantiated()
    {
        // Act
        var dto = new UnitInfoDto();

        // Assert
        Assert.NotNull(dto);
    }

    [Fact]
    public void UnitInfoDto_CanSetAllProperties()
    {
        // Arrange
        var dto = new UnitInfoDto
        {
            Symbol = "kg",
            Name = "kilogram",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI"
        };

        // Assert
        Assert.Equal("kg", dto.Symbol);
        Assert.Equal("kilogram", dto.Name);
        Assert.True(dto.IsBaseUnit);
        Assert.True(dto.IsSIUnit);
        Assert.Equal("SI", dto.UnitSystem);
    }

    [Fact]
    public void UnitInfoDto_CanSetIsBaseUnitToFalse()
    {
        // Arrange
        var dto = new UnitInfoDto
        {
            Symbol = "g",
            Name = "gram",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI"
        };

        // Assert
        Assert.False(dto.IsBaseUnit);
    }

    [Fact]
    public void UnitInfoDto_CanSetIsSIUnitToFalse()
    {
        // Arrange
        var dto = new UnitInfoDto
        {
            Symbol = "lb",
            Name = "pound",
            IsBaseUnit = false,
            IsSIUnit = false,
            UnitSystem = "Imperial"
        };

        // Assert
        Assert.False(dto.IsSIUnit);
        Assert.Equal("Imperial", dto.UnitSystem);
    }

    #endregion
}

