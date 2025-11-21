namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

public class UnitConvertToBaseTests
{
    [Fact]
    public void ConvertToBase_WhenUnitHasFormula_ThrowsInvalidOperationException()
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
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertToBase(25.0));
        Assert.Contains("Formula-based conversion not supported", exception.Message);
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
            ConversionFactor = 0.0
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => unit.ConvertFromBase(10.0));
        Assert.Contains("No conversion method available", exception.Message);
    }
}

