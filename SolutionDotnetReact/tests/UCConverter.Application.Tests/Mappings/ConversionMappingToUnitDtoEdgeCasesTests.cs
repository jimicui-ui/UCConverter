namespace UCConverter.Application.Tests.Mappings;

using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Application.Mappings;
using UCConverter.Domain.Entities;
using Xunit;

public class ConversionMappingToUnitDtoEdgeCasesTests
{
    [Fact]
    public void ToUnitDto_WhenLocalizationServiceNotNullButCategoryNameIsNull_UsesUnitDisplayName()
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
        var dto = unit.ToUnitDto(mockLocalizationService.Object, null);

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("Meter", dto.DisplayName); // Should use unit.DisplayName, not call localization service
        mockLocalizationService.Verify(l => l.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ToUnitDto_WhenLocalizationServiceNotNullButCategoryNameIsEmpty_UsesUnitDisplayName()
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
        var dto = unit.ToUnitDto(mockLocalizationService.Object, "");

        // Assert
        Assert.NotNull(dto);
        Assert.Equal("Meter", dto.DisplayName); // Should use unit.DisplayName, not call localization service
        mockLocalizationService.Verify(l => l.GetUnitDisplayName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void ToUnitDto_WhenLocalizationServiceNotNullButCategoryNameIsWhitespace_CallsLocalizationService()
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
        mockLocalizationService.Setup(l => l.GetUnitDisplayName("   ", "m", "Meter"))
            .Returns("Meter");

        // Act
        var dto = unit.ToUnitDto(mockLocalizationService.Object, "   ");

        // Assert
        Assert.NotNull(dto);
        // Note: string.IsNullOrEmpty("   ") returns false, so localization service is called
        mockLocalizationService.Verify(l => l.GetUnitDisplayName("   ", "m", "Meter"), Times.Once);
    }
}

