namespace UCConverter.Domain.Tests.Entities;

using UCConverter.Domain.Entities;
using Xunit;

public class CategoryValidationTests
{
    [Fact]
    public void ValidateUnits_WhenBothUnitsExist_ReturnsTrue()
    {
        // Arrange
        var unit1 = new Unit { Symbol = "m", Name = "meter" };
        var unit2 = new Unit { Symbol = "km", Name = "kilometer" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { unit1, unit2 }
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
        var unit2 = new Unit { Symbol = "km", Name = "kilometer" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { unit2 }
        };

        // Act
        var result = category.ValidateUnits("nonexistent", "km");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUnits_WhenToUnitDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var unit1 = new Unit { Symbol = "m", Name = "meter" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { unit1 }
        };

        // Act
        var result = category.ValidateUnits("m", "nonexistent");

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
        var result = category.ValidateUnits("nonexistent1", "nonexistent2");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUnits_WhenCaseInsensitive_ReturnsTrue()
    {
        // Arrange
        var unit1 = new Unit { Symbol = "m", Name = "meter" };
        var unit2 = new Unit { Symbol = "km", Name = "kilometer" };
        var category = new Category
        {
            Name = "length",
            Units = new List<Unit> { unit1, unit2 }
        };

        // Act
        var result = category.ValidateUnits("M", "KM");

        // Assert
        Assert.True(result);
    }
}

