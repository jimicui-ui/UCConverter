namespace UCConverter.Infrastructure.Tests.Data;

using UCConverter.Infrastructure.Data;
using Xunit;

/// <summary>
/// Complete tests for UnitCategoryJson and UnitJson to ensure 100% coverage
/// </summary>
public class UnitCategoryJsonCompleteTests
{
    [Fact]
    public void UnitCategoryJson_CanSetAndGetAllProperties()
    {
        // Arrange & Act
        var categoryJson = new UnitCategoryJson
        {
            Category = "test",
            CategoryDisplayName = "Test Category",
            Group = "Common",
            BaseUnit = new UnitJson
            {
                Symbol = "t",
                Name = "test",
                DisplayName = "Test",
                Category = "test",
                IsBaseUnit = true,
                IsSIUnit = true,
                UnitSystem = "SI",
                ConversionFactor = 1.0,
                ConversionFormula = "x + 273.15",
                ConversionInverseFormula = "x - 273.15"
            },
            Units = new List<UnitJson>
            {
                new UnitJson
                {
                    Symbol = "t1",
                    Name = "test1",
                    DisplayName = "Test 1",
                    Category = "test",
                    IsBaseUnit = false,
                    IsSIUnit = true,
                    UnitSystem = "SI",
                    ConversionFactor = 2.0
                }
            }
        };

        // Assert
        Assert.Equal("test", categoryJson.Category);
        Assert.Equal("Test Category", categoryJson.CategoryDisplayName);
        Assert.Equal("Common", categoryJson.Group);
        Assert.NotNull(categoryJson.BaseUnit);
        Assert.Single(categoryJson.Units);
    }

    [Fact]
    public void UnitCategoryJson_CanSetGroupToNull()
    {
        // Arrange & Act
        var categoryJson = new UnitCategoryJson
        {
            Category = "test",
            CategoryDisplayName = "Test Category",
            Group = null,
            BaseUnit = new UnitJson(),
            Units = new List<UnitJson>()
        };

        // Assert
        Assert.Null(categoryJson.Group);
    }

    [Fact]
    public void UnitCategoryJson_CanInitializeWithEmptyUnits()
    {
        // Arrange & Act
        var categoryJson = new UnitCategoryJson
        {
            Category = "test",
            CategoryDisplayName = "Test Category",
            BaseUnit = new UnitJson(),
            Units = new List<UnitJson>()
        };

        // Assert
        Assert.NotNull(categoryJson.Units);
        Assert.Empty(categoryJson.Units);
    }

    [Fact]
    public void UnitJson_CanSetAndGetAllProperties()
    {
        // Arrange & Act
        var unitJson = new UnitJson
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            Category = "length",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = "x - 273.15"
        };

        // Assert
        Assert.Equal("m", unitJson.Symbol);
        Assert.Equal("meter", unitJson.Name);
        Assert.Equal("Meter", unitJson.DisplayName);
        Assert.Equal("length", unitJson.Category);
        Assert.True(unitJson.IsBaseUnit);
        Assert.True(unitJson.IsSIUnit);
        Assert.Equal("SI", unitJson.UnitSystem);
        Assert.Equal(1.0, unitJson.ConversionFactor);
        Assert.Equal("x + 273.15", unitJson.ConversionFormula);
        Assert.Equal("x - 273.15", unitJson.ConversionInverseFormula);
    }

    [Fact]
    public void UnitJson_CanSetConversionFactorToNull()
    {
        // Arrange & Act
        var unitJson = new UnitJson
        {
            Symbol = "C",
            Name = "celsius",
            DisplayName = "Celsius",
            Category = "temperature",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = null,
            ConversionFormula = "x + 273.15",
            ConversionInverseFormula = "x - 273.15"
        };

        // Assert
        Assert.Null(unitJson.ConversionFactor);
        Assert.NotNull(unitJson.ConversionFormula);
        Assert.NotNull(unitJson.ConversionInverseFormula);
    }

    [Fact]
    public void UnitJson_CanSetConversionFormulasToNull()
    {
        // Arrange & Act
        var unitJson = new UnitJson
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            Category = "length",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0,
            ConversionFormula = null,
            ConversionInverseFormula = null
        };

        // Assert
        Assert.Null(unitJson.ConversionFormula);
        Assert.Null(unitJson.ConversionInverseFormula);
    }

    [Fact]
    public void UnitJson_CanSetBooleanPropertiesToFalse()
    {
        // Arrange & Act
        var unitJson = new UnitJson
        {
            Symbol = "ft",
            Name = "foot",
            DisplayName = "Foot",
            Category = "length",
            IsBaseUnit = false,
            IsSIUnit = false,
            UnitSystem = "Imperial",
            ConversionFactor = 0.3048
        };

        // Assert
        Assert.False(unitJson.IsBaseUnit);
        Assert.False(unitJson.IsSIUnit);
    }

    [Fact]
    public void UnitCategoryJson_DefaultValuesAreCorrect()
    {
        // Arrange & Act
        var categoryJson = new UnitCategoryJson();

        // Assert
        Assert.Equal(string.Empty, categoryJson.Category);
        Assert.Equal(string.Empty, categoryJson.CategoryDisplayName);
        Assert.Null(categoryJson.Group);
        Assert.NotNull(categoryJson.Units);
        Assert.Empty(categoryJson.Units);
    }

    [Fact]
    public void UnitJson_DefaultValuesAreCorrect()
    {
        // Arrange & Act
        var unitJson = new UnitJson();

        // Assert
        Assert.Equal(string.Empty, unitJson.Symbol);
        Assert.Equal(string.Empty, unitJson.Name);
        Assert.Equal(string.Empty, unitJson.DisplayName);
        Assert.Equal(string.Empty, unitJson.Category);
        Assert.False(unitJson.IsBaseUnit);
        Assert.False(unitJson.IsSIUnit);
        Assert.Equal(string.Empty, unitJson.UnitSystem);
        Assert.Null(unitJson.ConversionFactor);
        Assert.Null(unitJson.ConversionFormula);
        Assert.Null(unitJson.ConversionInverseFormula);
    }
}

