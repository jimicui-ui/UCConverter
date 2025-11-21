namespace UCConverter.Infrastructure.Tests.Data;

using UCConverter.Infrastructure.Data;
using Xunit;

public class UnitCategoryJsonTests
{
    [Fact]
    public void UnitCategoryJson_CanBeCreated()
    {
        // Arrange & Act
        var categoryJson = new UnitCategoryJson
        {
            Category = "test",
            CategoryDisplayName = "Test Category",
            BaseUnit = new UnitJson
            {
                Symbol = "t",
                Name = "test",
                DisplayName = "Test",
                IsBaseUnit = true,
                IsSIUnit = true,
                UnitSystem = "SI"
            },
            Units = new List<UnitJson>()
        };

        // Assert
        Assert.NotNull(categoryJson);
        Assert.Equal("test", categoryJson.Category);
        Assert.NotNull(categoryJson.BaseUnit);
        Assert.NotNull(categoryJson.Units);
    }

    [Fact]
    public void UnitJson_CanBeCreated()
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
            ConversionFormula = null
        };

        // Assert
        Assert.NotNull(unitJson);
        Assert.Equal("m", unitJson.Symbol);
        Assert.Equal("meter", unitJson.Name);
        Assert.True(unitJson.IsBaseUnit);
        Assert.True(unitJson.IsSIUnit);
        Assert.Equal("SI", unitJson.UnitSystem);
        Assert.Equal(1.0, unitJson.ConversionFactor);
        Assert.Null(unitJson.ConversionFormula);
    }
}

