namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

public class UnitConvertToBaseEdgeCasesTests
{
    [Fact]
    public void ConvertToBase_WhenIsBaseUnit_ReturnsSameValue()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            IsBaseUnit = true,
            ConversionFactor = null
        };

        // Act
        var result = unit.ConvertToBase(100.0);

        // Assert
        Assert.Equal(100.0, result);
    }

    [Fact]
    public void ConvertToBase_WhenIsBaseUnitWithFactor_ReturnsSameValue()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            IsBaseUnit = true,
            ConversionFactor = 1000.0 // Even if factor exists, base unit returns same value
        };

        // Act
        var result = unit.ConvertToBase(100.0);

        // Assert
        Assert.Equal(100.0, result);
    }

    [Fact]
    public void ConvertToBase_WhenHasConversionFactor_ConvertsCorrectly()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            IsBaseUnit = false,
            ConversionFactor = 1000.0
        };

        // Act
        var result = unit.ConvertToBase(1.0);

        // Assert
        Assert.Equal(1000.0, result);
    }

    [Fact]
    public void ConvertFromBase_WhenIsBaseUnit_ReturnsSameValue()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            IsBaseUnit = true,
            ConversionFactor = null
        };

        // Act
        var result = unit.ConvertFromBase(100.0);

        // Assert
        Assert.Equal(100.0, result);
    }

    [Fact]
    public void ConvertFromBase_WhenIsBaseUnitWithFactor_ReturnsSameValue()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            IsBaseUnit = true,
            ConversionFactor = 1000.0 // Even if factor exists, base unit returns same value
        };

        // Act
        var result = unit.ConvertFromBase(100.0);

        // Assert
        Assert.Equal(100.0, result);
    }

    [Fact]
    public void ConvertFromBase_WhenHasConversionFactor_ConvertsCorrectly()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            IsBaseUnit = false,
            ConversionFactor = 1000.0
        };

        // Act
        var result = unit.ConvertFromBase(1000.0);

        // Assert
        Assert.Equal(1.0, result);
    }
}

