namespace UCConverter.Infrastructure.Tests.Data;

using System.Text.Json;
using UCConverter.Infrastructure.Data;
using Xunit;

public class UnitCategoryJsonTests
{
    [Fact]
    public void Deserialize_WhenValidJson_DeserializesCorrectly()
    {
        // Arrange
        var json = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""group"": ""Common"",
  ""baseUnit"": {
    ""symbol"": ""t"",
    ""name"": ""test"",
    ""displayName"": ""Test"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": [
    {
      ""symbol"": ""t"",
      ""name"": ""test"",
      ""displayName"": ""Test"",
      ""isBaseUnit"": true,
      ""isSIUnit"": true,
      ""unitSystem"": ""SI"",
      ""conversionFactor"": 1.0
    }
  ]
}";

        // Act
        var result = JsonSerializer.Deserialize<UnitCategoryJson>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result!.Category);
        Assert.Equal("Test Category", result.CategoryDisplayName);
        Assert.Equal("Common", result.Group);
        Assert.NotNull(result.BaseUnit);
        Assert.NotNull(result.Units);
        Assert.Single(result.Units);
    }

    [Fact]
    public void Deserialize_WhenCaseInsensitive_DeserializesCorrectly()
    {
        // Arrange
        var json = @"{
  ""CATEGORY"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""GROUP"": ""Common"",
  ""baseUnit"": {
    ""SYMBOL"": ""t"",
    ""name"": ""test"",
    ""displayName"": ""Test"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": []
}";

        // Act
        var result = JsonSerializer.Deserialize<UnitCategoryJson>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result!.Category);
        Assert.Equal("Common", result.Group);
    }

    [Fact]
    public void Deserialize_WhenUnitsIsNull_HandlesCorrectly()
    {
        // Arrange
        var json = @"{
  ""category"": ""test"",
  ""categoryDisplayName"": ""Test Category"",
  ""group"": ""Common"",
  ""baseUnit"": {
    ""symbol"": ""t"",
    ""name"": ""test"",
    ""displayName"": ""Test"",
    ""isBaseUnit"": true,
    ""isSIUnit"": true,
    ""unitSystem"": ""SI"",
    ""conversionFactor"": 1.0
  },
  ""units"": null
}";

        // Act
        var result = JsonSerializer.Deserialize<UnitCategoryJson>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        Assert.NotNull(result);
        Assert.Null(result!.Units);
    }
}
