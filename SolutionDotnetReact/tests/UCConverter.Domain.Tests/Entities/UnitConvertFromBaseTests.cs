namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

public class UnitConvertFromBaseTests
{
    [Fact]
    public void ConvertFromBase_WhenUnitHasFormula_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "°C",
            Name = "celsius",
            Category = "temperature",
            ConversionFormula = "x + 273.15"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(273.15));
        Assert.Contains("Formula-based conversion not supported", exception.Message);
    }

    [Fact]
    public void ConvertFromBase_WhenUnitHasNoConversionMethod_ThrowsInvalidOperationException()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "invalid",
            Name = "invalid",
            Category = "test",
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
            Category = "test",
            IsBaseUnit = false,
            ConversionFactor = 0.0,
            ConversionFormula = null
        };

        // Act & Assert
        // When factor is 0, it skips division (due to && ConversionFactor.Value != 0 check)
        // and goes to formula check, then throws
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(10.0));
        Assert.Contains("No conversion method available", exception.Message);
    }
}

