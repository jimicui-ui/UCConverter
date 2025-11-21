namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

public class UnitTests
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
            ConversionFactor = 1.0
        };

        // Act
        var result = unit.ConvertToBase(10.0);

        // Assert
        Assert.Equal(10.0, result);
    }

    [Fact]
    public void ConvertToBase_WhenHasConversionFactor_ReturnsConvertedValue()
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
        var result = unit.ConvertToBase(5.0);

        // Assert
        Assert.Equal(5000.0, result);
    }

    [Fact]
    public void ConvertToBase_WhenHasFormula_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            IsBaseUnit = false,
            ConversionFormula = "x + 273.15"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertToBase(25.0));
        Assert.Contains("Formula-based conversion not supported", exception.Message);
    }

    [Fact]
    public void ConvertToBase_WhenNoConversionMethod_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "unknown",
            Name = "unknown",
            IsBaseUnit = false,
            ConversionFactor = null,
            ConversionFormula = null
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertToBase(10.0));
        Assert.Contains("No conversion method available", exception.Message);
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
            ConversionFactor = 1.0
        };

        // Act
        var result = unit.ConvertFromBase(10.0);

        // Assert
        Assert.Equal(10.0, result);
    }

    [Fact]
    public void ConvertFromBase_WhenHasConversionFactor_ReturnsConvertedValue()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "cm",
            Name = "centimeter",
            IsBaseUnit = false,
            ConversionFactor = 0.01
        };

        // Act
        var result = unit.ConvertFromBase(1.0);

        // Assert
        Assert.Equal(100.0, result);
    }

    [Fact]
    public void ConvertFromBase_WhenHasFormula_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "°F",
            Name = "fahrenheit",
            IsBaseUnit = false,
            ConversionFormula = "(x - 32) * 5/9 + 273.15"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(273.15));
        Assert.Contains("Formula-based conversion not supported", exception.Message);
    }

    [Fact]
    public void ConvertFromBase_WhenNoConversionMethod_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "unknown",
            Name = "unknown",
            IsBaseUnit = false,
            ConversionFactor = null,
            ConversionFormula = null
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(10.0));
        Assert.Contains("No conversion method available", exception.Message);
    }

    [Fact]
    public void ConvertFromBase_WhenConversionFactorIsZero_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "invalid",
            Name = "invalid",
            IsBaseUnit = false,
            ConversionFactor = 0.0
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(10.0));
        Assert.Contains("No conversion method available", exception.Message);
    }
}

