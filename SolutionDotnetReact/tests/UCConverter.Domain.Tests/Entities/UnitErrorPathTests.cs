namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

public class UnitErrorPathTests
{
    [Fact]
    public void ConvertToBase_WhenNoConversionMethodAvailable_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "invalid",
            Name = "invalid",
            IsBaseUnit = false,
            ConversionFactor = null,
            ConversionFormula = null
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertToBase(100.0));
        Assert.Contains("No conversion method available", exception.Message);
        Assert.Contains("invalid", exception.Message);
    }

    [Fact]
    public void ConvertFromBase_WhenNoConversionMethodAvailable_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "invalid",
            Name = "invalid",
            IsBaseUnit = false,
            ConversionFactor = null,
            ConversionFormula = null
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(100.0));
        Assert.Contains("No conversion method available", exception.Message);
        Assert.Contains("invalid", exception.Message);
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
            ConversionFactor = 0.0,
            ConversionFormula = null
        };

        // Act & Assert
        // When factor is 0, it skips the division and goes to formula check, then throws
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(100.0));
        Assert.Contains("No conversion method available", exception.Message);
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
            ConversionFactor = null,
            ConversionFormula = "x + 273.15"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertToBase(25.0));
        Assert.Contains("Formula-based conversion not supported", exception.Message);
        Assert.Contains("°C", exception.Message);
    }

    [Fact]
    public void ConvertFromBase_WhenHasFormula_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            IsBaseUnit = false,
            ConversionFactor = null,
            ConversionFormula = "x + 273.15"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(298.15));
        Assert.Contains("Formula-based conversion not supported", exception.Message);
        Assert.Contains("°C", exception.Message);
    }
}

