namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

public class ConversionResultTests
{
    [Fact]
    public void ConversionResult_CanBeCreated()
    {
        // Arrange & Act
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

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1.0, result.Result);
        Assert.Equal("1 km", result.FormattedResult);
        Assert.Equal(4, result.Precision);
        Assert.Null(result.Formula);
        Assert.NotNull(result.FromUnit);
        Assert.NotNull(result.ToUnit);
        Assert.Equal(1000.0, result.OriginalValue);
    }

    [Fact]
    public void ConversionResult_WithFormula_CanBeCreated()
    {
        // Arrange & Act
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

        // Assert
        Assert.NotNull(result);
        Assert.Equal(298.15, result.Result);
        Assert.NotNull(result.Formula);
        Assert.Equal("x + 273.15", result.Formula);
    }
}

