namespace UCConverter.Application.Tests.Services;

using Microsoft.Extensions.Localization;
using Moq;
using UCConverter.Application.Resources;
using UCConverter.Application.Services;
using Xunit;

public class LocalizationServiceTests
{
    private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
    private readonly LocalizationService _service;

    public LocalizationServiceTests()
    {
        _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
        _service = new LocalizationService(_mockLocalizer.Object);
    }

    [Fact]
    public void Constructor_WhenLocalizerIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new LocalizationService(null!));
    }

    [Fact]
    public void GetString_WhenKeyExists_ReturnsLocalizedString()
    {
        // Arrange
        var localizedString = new LocalizedString("TestKey", "Test Value");
        _mockLocalizer.Setup(l => l["TestKey"]).Returns(localizedString);

        // Act
        var result = _service.GetString("TestKey");

        // Assert
        Assert.Equal("Test Value", result);
    }

    [Fact]
    public void GetString_WithArgs_FormatsString()
    {
        // Arrange
        var localizedString = new LocalizedString("TestKey", "Hello {0}");
        _mockLocalizer.Setup(l => l["TestKey"]).Returns(localizedString);

        // Act
        var result = _service.GetString("TestKey", "World");

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenTranslationExists_ReturnsLocalizedName()
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
    public void GetCategoryDisplayName_WhenTranslationNotFound_ReturnsCapitalizedCategoryName()
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
    public void GetCategoryDisplayName_WhenCategoryNameIsEmpty_ReturnsEmptyString()
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
    public void GetUnitDisplayName_WhenTranslationExists_ReturnsLocalizedName()
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
    public void GetErrorMessage_WhenTranslationExists_ReturnsLocalizedError()
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
    public void GetErrorMessage_WhenTranslationNotFound_ReturnsDefaultError()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_UnknownError", "Error_UnknownError", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_UnknownError"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("UnknownError");

        // Assert
        Assert.Equal("An error occurred", result);
    }

    [Fact]
    public void GetErrorMessage_WithCategoryNotFoundKey_ReturnsDefaultCategoryNotFoundMessage()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_CategoryNotFound", "Error_CategoryNotFound", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_CategoryNotFound"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("CategoryNotFound", "test");

        // Assert
        Assert.Equal("Category 'test' not found", result);
    }

    [Fact]
    public void GetErrorMessage_WithUnitNotFoundKey_ReturnsDefaultUnitNotFoundMessage()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_UnitNotFound", "Error_UnitNotFound", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_UnitNotFound"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("UnitNotFound", "m");

        // Assert
        Assert.Equal("Unit 'm' not found", result);
    }

    [Fact]
    public void GetErrorMessage_WithInvalidConversionKey_ReturnsDefaultInvalidConversionMessage()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_InvalidConversion", "Error_InvalidConversion", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_InvalidConversion"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("InvalidConversion");

        // Assert
        Assert.Equal("Invalid conversion", result);
    }

    [Fact]
    public void GetErrorMessage_WithInvalidInputKey_ReturnsDefaultInvalidInputMessage()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_InvalidInput", "Error_InvalidInput", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_InvalidInput"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("InvalidInput");

        // Assert
        Assert.Equal("Invalid input", result);
    }

    [Fact]
    public void GetErrorMessage_WithoutArgs_ReturnsLocalizedString()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_Test", "Test Error");
        _mockLocalizer.Setup(l => l["Error_Test"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("Test");

        // Assert
        Assert.Equal("Test Error", result);
    }

    [Fact]
    public void GetString_WithoutArgs_ReturnsLocalizedString()
    {
        // Arrange
        var localizedString = new LocalizedString("TestKey", "Test Value");
        _mockLocalizer.Setup(l => l["TestKey"]).Returns(localizedString);

        // Act
        var result = _service.GetString("TestKey");

        // Assert
        Assert.Equal("Test Value", result);
    }

    [Fact]
    public void GetString_WithEmptyArgsArray_ReturnsLocalizedString()
    {
        // Arrange
        var localizedString = new LocalizedString("TestKey", "Test Value");
        _mockLocalizer.Setup(l => l["TestKey"]).Returns(localizedString);

        // Act
        var result = _service.GetString("TestKey", Array.Empty<object>());

        // Assert
        Assert.Equal("Test Value", result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenCategoryNameIsSingleCharacter_ReturnsCapitalized()
    {
        // Arrange
        var localizedString = new LocalizedString("Category_a", "", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Category_a"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName("a");

        // Assert
        Assert.Equal("A", result);
    }

    [Fact]
    public void GetErrorMessage_WithEmptyArgsArray_ReturnsLocalizedString()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_Test", "Test Error");
        _mockLocalizer.Setup(l => l["Error_Test"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("Test", Array.Empty<object>());

        // Assert
        Assert.Equal("Test Error", result);
    }

    [Fact]
    public void GetErrorMessage_WithMultipleArgs_FormatsCorrectly()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_Test", "Error {0} and {1}");
        _mockLocalizer.Setup(l => l["Error_Test"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("Test", "arg1", "arg2");

        // Assert
        Assert.Equal("Error arg1 and arg2", result);
    }
}

