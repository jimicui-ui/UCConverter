namespace UCConverter.Application.Tests.Services;

using Microsoft.Extensions.Localization;
using Moq;
using UCConverter.Application.Resources;
using UCConverter.Application.Services;
using Xunit;

public class LocalizationServiceComprehensiveTests
{
    private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
    private readonly LocalizationService _service;

    public LocalizationServiceComprehensiveTests()
    {
        _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
        _service = new LocalizationService(_mockLocalizer.Object);
    }

    [Fact]
    public void GetString_WhenKeyExists_ReturnsLocalizedString()
    {
        // Arrange
        var localizedString = new LocalizedString("Key", "Value");
        _mockLocalizer.Setup(l => l["Key"]).Returns(localizedString);

        // Act
        var result = _service.GetString("Key");

        // Assert
        Assert.Equal("Value", result);
    }

    [Fact]
    public void GetString_WhenKeyExistsWithArgs_FormatsString()
    {
        // Arrange
        var localizedString = new LocalizedString("Key", "Hello {0}");
        _mockLocalizer.Setup(l => l["Key"]).Returns(localizedString);

        // Act
        var result = _service.GetString("Key", "World");

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void GetString_WhenKeyExistsWithMultipleArgs_FormatsString()
    {
        // Arrange
        var localizedString = new LocalizedString("Key", "{0} and {1}");
        _mockLocalizer.Setup(l => l["Key"]).Returns(localizedString);

        // Act
        var result = _service.GetString("Key", "First", "Second");

        // Assert
        Assert.Equal("First and Second", result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenTranslationExists_ReturnsLocalizedValue()
    {
        // Arrange
        var localizedString = new LocalizedString("Category_length", "Length / Distance");
        _mockLocalizer.Setup(l => l["Category_length"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName("length");

        // Assert
        Assert.Equal("Length / Distance", result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenTranslationNotFound_ReturnsCapitalized()
    {
        // Arrange
        var localizedString = new LocalizedString("Category_unknown", "Category_unknown", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Category_unknown"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName("unknown");

        // Assert
        Assert.Equal("Unknown", result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenCategoryNameIsEmpty_ReturnsEmpty()
    {
        // Arrange
        var localizedString = new LocalizedString("Category_", "", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Category_"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName("");

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenCategoryNameIsNull_ReturnsNull()
    {
        // Arrange
        var localizedString = new LocalizedString("Category_", "", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Category_"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName(null!);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetUnitDisplayName_WhenTranslationExists_ReturnsLocalizedValue()
    {
        // Arrange
        var localizedString = new LocalizedString("Unit_length_m", "Meter");
        _mockLocalizer.Setup(l => l["Unit_length_m"]).Returns(localizedString);

        // Act
        var result = _service.GetUnitDisplayName("length", "m", "meter");

        // Assert
        Assert.Equal("Meter", result);
    }

    [Fact]
    public void GetUnitDisplayName_WhenTranslationNotFound_ReturnsDefaultName()
    {
        // Arrange
        var localizedString = new LocalizedString("Unit_length_unknown", "Unit_length_unknown", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Unit_length_unknown"]).Returns(localizedString);

        // Act
        var result = _service.GetUnitDisplayName("length", "unknown", "Default Unit");

        // Assert
        Assert.Equal("Default Unit", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationExists_ReturnsLocalizedValue()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_CategoryNotFound", "Category '{0}' not found");
        _mockLocalizer.Setup(l => l["Error_CategoryNotFound"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("CategoryNotFound", "test");

        // Assert
        Assert.Equal("Category 'test' not found", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationExistsWithoutArgs_ReturnsLocalizedValue()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_InvalidConversion", "Invalid conversion");
        _mockLocalizer.Setup(l => l["Error_InvalidConversion"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("InvalidConversion");

        // Assert
        Assert.Equal("Invalid conversion", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_ReturnsDefaultMessage()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_Unknown", "Error_Unknown", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_Unknown"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("Unknown");

        // Assert
        Assert.Equal("An error occurred", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFoundWithArgs_ReturnsDefaultMessage()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_CategoryNotFound", "Error_CategoryNotFound", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_CategoryNotFound"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("CategoryNotFound", "test");

        // Assert
        Assert.Equal("Category 'test' not found", result);
    }
}

