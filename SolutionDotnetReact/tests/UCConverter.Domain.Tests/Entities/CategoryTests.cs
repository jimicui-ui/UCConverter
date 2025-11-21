namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

public class CategoryTests
{
    [Fact]
    public void GetUnitBySymbol_WhenUnitExists_ReturnsUnit()
    {
        // Arrange
        var unit = new Unit { Symbol = "m", Name = "meter" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { unit }
        };

        // Act
        var result = category.GetUnitBySymbol("m");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("m", result.Symbol);
    }

    [Fact]
    public void GetUnitBySymbol_WhenUnitDoesNotExist_ReturnsNull()
    {
        // Arrange
        var unit = new Unit { Symbol = "m", Name = "meter" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { unit }
        };

        // Act
        var result = category.GetUnitBySymbol("km");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUnitBySymbol_WhenCaseDifferent_ReturnsUnit()
    {
        // Arrange
        var unit = new Unit { Symbol = "m", Name = "meter" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { unit }
        };

        // Act
        var result = category.GetUnitBySymbol("M");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("m", result.Symbol);
    }

    [Fact]
    public void ValidateUnits_WhenBothUnitsExist_ReturnsTrue()
    {
        // Arrange
        var fromUnit = new Unit { Symbol = "m", Name = "meter" };
        var toUnit = new Unit { Symbol = "km", Name = "kilometer" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { fromUnit, toUnit }
        };

        // Act
        var result = category.ValidateUnits("m", "km");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateUnits_WhenFromUnitDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var toUnit = new Unit { Symbol = "km", Name = "kilometer" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { toUnit }
        };

        // Act
        var result = category.ValidateUnits("m", "km");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUnits_WhenToUnitDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var fromUnit = new Unit { Symbol = "m", Name = "meter" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { fromUnit }
        };

        // Act
        var result = category.ValidateUnits("m", "km");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUnits_WhenBothUnitsDoNotExist_ReturnsFalse()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit>()
        };

        // Act
        var result = category.ValidateUnits("m", "km");

        // Assert
        Assert.False(result);
    }
}

