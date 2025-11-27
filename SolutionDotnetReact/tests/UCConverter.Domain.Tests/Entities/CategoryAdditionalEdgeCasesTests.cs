namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

/// <summary>
/// Additional edge case tests for Category entity
/// </summary>
public class CategoryAdditionalEdgeCasesTests
{
    [Fact]
    public void GetUnitBySymbol_WhenSymbolIsNull_ReturnsNull()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" }
            }
        };

        // Act
        var result = category.GetUnitBySymbol(null!);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUnitBySymbol_WhenSymbolIsEmpty_ReturnsNull()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" }
            }
        };

        // Act
        var result = category.GetUnitBySymbol("");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUnitBySymbol_WhenSymbolIsWhitespace_ReturnsNull()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" }
            }
        };

        // Act
        var result = category.GetUnitBySymbol("   ");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUnitBySymbol_WhenCaseDifferent_ReturnsUnit()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" },
                new Unit { Symbol = "km", Name = "kilometer" }
            }
        };

        // Act
        var result = category.GetUnitBySymbol("KM");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("km", result.Symbol);
    }

    [Fact]
    public void ValidateUnits_WhenFromUnitIsNull_ReturnsFalse()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" }
            }
        };

        // Act
        var result = category.ValidateUnits(null!, "m");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUnits_WhenToUnitIsNull_ReturnsFalse()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" }
            }
        };

        // Act
        var result = category.ValidateUnits("m", null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUnits_WhenBothUnitsAreNull_ReturnsFalse()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" }
            }
        };

        // Act
        var result = category.ValidateUnits(null!, null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUnits_WhenBothUnitsExist_ReturnsTrue()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" },
                new Unit { Symbol = "km", Name = "kilometer" }
            }
        };

        // Act
        var result = category.ValidateUnits("m", "km");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateUnits_WhenUnitsAreCaseDifferent_ReturnsTrue()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length",
            Group = "Common",
            BaseUnit = new Unit { Symbol = "m", Name = "meter" },
            Units = new List<Unit>
            {
                new Unit { Symbol = "m", Name = "meter" },
                new Unit { Symbol = "km", Name = "kilometer" }
            }
        };

        // Act
        var result = category.ValidateUnits("M", "KM");

        // Assert
        Assert.True(result);
    }
}

