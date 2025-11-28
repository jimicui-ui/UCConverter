namespace UCConverter.Application.Tests.Mappings;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Mappings;
using UCConverter.Domain.Entities;
using Xunit;

public class ConversionMappingTests
{
    [Fact]
    public void ToUnitInfoDto_WhenValidUnit_ReturnsDto()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI"
        };

        // Act
        var result = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("m", result.Symbol);
        Assert.Equal("meter", result.Name);
        Assert.True(result.IsBaseUnit);
        Assert.True(result.IsSIUnit);
        Assert.Equal("SI", result.UnitSystem);
    }

    [Fact]
    public void ToConvertResponseDto_WhenValidResult_ReturnsDto()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 1.0,
            FormattedResult = "1 km",
            Precision = 4,
            Formula = null,
            FromUnit = new Unit { Symbol = "m", Name = "meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI" },
            ToUnit = new Unit { Symbol = "km", Name = "kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI" },
            OriginalValue = 1000.0
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(1.0, dto.Result);
        Assert.Equal("1 km", dto.FormattedResult);
        Assert.Equal(4, dto.Precision);
        Assert.Null(dto.Formula);
        Assert.NotNull(dto.FromUnit);
        Assert.NotNull(dto.ToUnit);
    }

    [Fact]
    public void ToCategoryDto_WhenValidCategory_ReturnsDto()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length / Distance"
        };

        // Act
        var result = category.ToCategoryDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("length", result.Name);
        Assert.Equal("Length / Distance", result.DisplayName);
    }

    [Fact]
    public void ToCategoryDto_WithLocalizationService_ReturnsLocalizedDisplayName()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length / Distance"
        };
        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(l => l.GetCategoryDisplayName("length"))
            .Returns("长度 / 距离");

        // Act
        var result = category.ToCategoryDto(mockLocalizationService.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("length", result.Name);
        Assert.Equal("长度 / 距离", result.DisplayName);
    }

    [Fact]
    public void ToCategoryDto_WithoutLocalizationService_ReturnsOriginalDisplayName()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length / Distance"
        };

        // Act
        var result = category.ToCategoryDto(null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("length", result.Name);
        Assert.Equal("Length / Distance", result.DisplayName);
    }

    [Fact]
    public void ToUnitDto_WhenValidUnit_ReturnsDto()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };

        // Act
        var result = unit.ToUnitDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("m", result.Symbol);
        Assert.Equal("meter", result.Name);
        Assert.Equal("Meter", result.DisplayName);
        Assert.True(result.IsBaseUnit);
        Assert.True(result.IsSIUnit);
        Assert.Equal("SI", result.UnitSystem);
        Assert.Equal(1.0, result.ConversionFactor);
    }

    [Fact]
    public void ToUnitDto_WithLocalizationService_ReturnsLocalizedDisplayName()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };
        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(l => l.GetUnitDisplayName("length", "m", "Meter"))
            .Returns("米");

        // Act
        var result = unit.ToUnitDto(mockLocalizationService.Object, "length");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("m", result.Symbol);
        Assert.Equal("米", result.DisplayName);
    }

    [Fact]
    public void ToUnitDto_WithoutLocalizationService_ReturnsOriginalDisplayName()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };

        // Act
        var result = unit.ToUnitDto(null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meter", result.DisplayName);
    }

    [Fact]
    public void ToUnitDto_WithLocalizationServiceButEmptyCategoryName_ReturnsOriginalDisplayName()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };
        var mockLocalizationService = new Mock<ILocalizationService>();

        // Act
        var result = unit.ToUnitDto(mockLocalizationService.Object, "");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meter", result.DisplayName);
        mockLocalizationService.Verify(l => l.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ToUnitDto_WithLocalizationServiceButNullCategoryName_ReturnsOriginalDisplayName()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "m",
            Name = "meter",
            DisplayName = "Meter",
            IsBaseUnit = true,
            IsSIUnit = true,
            UnitSystem = "SI",
            ConversionFactor = 1.0
        };
        var mockLocalizationService = new Mock<ILocalizationService>();

        // Act
        var result = unit.ToUnitDto(mockLocalizationService.Object, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meter", result.DisplayName);
        mockLocalizationService.Verify(l => l.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ToConvertResponseDto_WhenResultHasFormula_ReturnsDtoWithFormula()
    {
        // Arrange
        var result = new ConversionResult
        {
            Result = 77.0,
            FormattedResult = "77 °F",
            Precision = 4,
            Formula = "x * 9/5 + 32",
            FromUnit = new Unit { Symbol = "°C", Name = "celsius", IsBaseUnit = false, IsSIUnit = false, UnitSystem = "Metric" },
            ToUnit = new Unit { Symbol = "°F", Name = "fahrenheit", IsBaseUnit = false, IsSIUnit = false, UnitSystem = "Imperial" },
            OriginalValue = 25.0
        };

        // Act
        var dto = result.ToConvertResponseDto();

        // Assert
        Assert.NotNull(dto);
        Assert.Equal(77.0, dto.Result);
        Assert.Equal("77 °F", dto.FormattedResult);
        Assert.Equal("x * 9/5 + 32", dto.Formula);
        Assert.NotNull(dto.FromUnit);
        Assert.NotNull(dto.ToUnit);
    }

    [Fact]
    public void ToCategoryDto_WhenCategoryHasGroup_ReturnsDtoWithGroup()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length / Distance",
            Group = "Common"
        };

        // Act
        var result = category.ToCategoryDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("length", result.Name);
        Assert.Equal("Length / Distance", result.DisplayName);
        Assert.Equal("Common", result.Group);
    }

    [Fact]
    public void ToCategoryDto_WhenCategoryHasGroupWithLocalization_ReturnsLocalizedDtoWithGroup()
    {
        // Arrange
        var category = new Category
        {
            Name = "length",
            DisplayName = "Length / Distance",
            Group = "Common"
        };
        var mockLocalizationService = new Mock<ILocalizationService>();
        mockLocalizationService.Setup(l => l.GetCategoryDisplayName("length"))
            .Returns("长度 / 距离");

        // Act
        var result = category.ToCategoryDto(mockLocalizationService.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("length", result.Name);
        Assert.Equal("长度 / 距离", result.DisplayName);
        Assert.Equal("Common", result.Group);
    }

    [Fact]
    public void ToUnitInfoDto_WhenUnitHasAllProperties_ReturnsCompleteDto()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "km",
            Name = "kilometer",
            IsBaseUnit = false,
            IsSIUnit = true,
            UnitSystem = "SI"
        };

        // Act
        var result = unit.ToUnitInfoDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("km", result.Symbol);
        Assert.Equal("kilometer", result.Name);
        Assert.False(result.IsBaseUnit);
        Assert.True(result.IsSIUnit);
        Assert.Equal("SI", result.UnitSystem);
    }

    [Fact]
    public void ToUnitDto_WhenUnitHasAllProperties_ReturnsCompleteDto()
    {
        // Arrange
        var unit = new Unit
        {
            Symbol = "ft",
            Name = "foot",
            DisplayName = "Foot",
            IsBaseUnit = false,
            IsSIUnit = false,
            UnitSystem = "Imperial",
            ConversionFactor = 0.3048
        };

        // Act
        var result = unit.ToUnitDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ft", result.Symbol);
        Assert.Equal("foot", result.Name);
        Assert.Equal("Foot", result.DisplayName);
        Assert.False(result.IsBaseUnit);
        Assert.False(result.IsSIUnit);
        Assert.Equal("Imperial", result.UnitSystem);
        Assert.Equal(0.3048, result.ConversionFactor);
    }
}

