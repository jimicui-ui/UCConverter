namespace UCConverter.Application.Tests.Services;

using Microsoft.Extensions.Localization;
using Moq;
using UCConverter.Application.Resources;
using UCConverter.Application.Services;
using Xunit;

/// <summary>
/// Comprehensive tests to achieve 100% code coverage for LocalizationService
/// </summary>
public class LocalizationServiceCompleteCoverageTests
{
    private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
    private readonly LocalizationService _service;

    public LocalizationServiceCompleteCoverageTests()
    {
        _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
        _service = new LocalizationService(_mockLocalizer.Object);
    }

    #region GetString Tests

    [Fact]
    public void GetString_WhenNoArgs_ReturnsValueDirectly()
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
    public void GetString_WhenArgsProvided_FormatsString()
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
    public void GetString_WhenArgsLengthIsZero_ReturnsValueDirectly()
    {
        // Arrange
        var localizedString = new LocalizedString("Key", "Value");
        _mockLocalizer.Setup(l => l["Key"]).Returns(localizedString);

        // Act
        var result = _service.GetString("Key", Array.Empty<object>());

        // Assert
        Assert.Equal("Value", result);
    }

    #endregion

    #region GetCategoryDisplayName Tests

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
    public void GetCategoryDisplayName_WhenTranslationFound_ReturnsLocalizedValue()
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
    public void GetCategoryDisplayName_WhenSingleCharacter_ReturnsCapitalized()
    {
        // Arrange
        var localizedString = new LocalizedString("Category_a", "Category_a", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Category_a"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName("a");

        // Assert
        Assert.Equal("A", result);
    }

    #endregion

    #region GetUnitDisplayName Tests

    [Fact]
    public void GetUnitDisplayName_WhenTranslationFound_ReturnsLocalizedValue()
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

    #endregion

    #region GetErrorMessage Tests - All Switch Cases

    [Fact]
    public void GetErrorMessage_WhenTranslationFound_ReturnsLocalizedValue()
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
    public void GetErrorMessage_WhenTranslationFoundWithArgs_FormatsString()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_Test", "Error {0}");
        _mockLocalizer.Setup(l => l["Error_Test"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("Test", "arg1");

        // Assert
        Assert.Equal("Error arg1", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_CategoryNotFound_ReturnsDefault()
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
    public void GetErrorMessage_WhenTranslationNotFound_UnitNotFound_ReturnsDefault()
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
    public void GetErrorMessage_WhenTranslationNotFound_InvalidConversion_ReturnsDefault()
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
    public void GetErrorMessage_WhenTranslationNotFound_InvalidInput_ReturnsDefault()
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
    public void GetErrorMessage_WhenTranslationNotFound_RequestBodyRequired_ReturnsDefault()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_RequestBodyRequired", "Error_RequestBodyRequired", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_RequestBodyRequired"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("RequestBodyRequired");

        // Assert
        Assert.Equal("Request body is required", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_CategoryRequired_ReturnsDefault()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_CategoryRequired", "Error_CategoryRequired", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_CategoryRequired"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("CategoryRequired");

        // Assert
        Assert.Equal("Category is required", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_FromUnitRequired_ReturnsDefault()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_FromUnitRequired", "Error_FromUnitRequired", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_FromUnitRequired"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("FromUnitRequired");

        // Assert
        Assert.Equal("FromUnit is required", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_ToUnitRequired_ReturnsDefault()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_ToUnitRequired", "Error_ToUnitRequired", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_ToUnitRequired"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("ToUnitRequired");

        // Assert
        Assert.Equal("ToUnit is required", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_InternalServerError_ReturnsDefault()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_InternalServerError", "Error_InternalServerError", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_InternalServerError"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("InternalServerError");

        // Assert
        Assert.Equal("An error occurred while processing the request", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_InternalServerErrorConversion_ReturnsDefault()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_InternalServerErrorConversion", "Error_InternalServerErrorConversion", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_InternalServerErrorConversion"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("InternalServerErrorConversion");

        // Assert
        Assert.Equal("An error occurred while performing the conversion", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_InternalServerErrorCategories_ReturnsDefault()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_InternalServerErrorCategories", "Error_InternalServerErrorCategories", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_InternalServerErrorCategories"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("InternalServerErrorCategories");

        // Assert
        Assert.Equal("An error occurred while retrieving categories", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_InternalServerErrorUnits_ReturnsDefault()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_InternalServerErrorUnits", "Error_InternalServerErrorUnits", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_InternalServerErrorUnits"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("InternalServerErrorUnits");

        // Assert
        Assert.Equal("An error occurred while retrieving units", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationNotFound_UnknownKey_ReturnsGenericMessage()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_UnknownKey", "Error_UnknownKey", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_UnknownKey"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("UnknownKey");

        // Assert
        Assert.Equal("An error occurred", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationFound_ArgsLengthIsZero_ReturnsValueDirectly()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_Test", "Test Error");
        _mockLocalizer.Setup(l => l["Error_Test"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("Test", Array.Empty<object>());

        // Assert
        Assert.Equal("Test Error", result);
    }

    #endregion
}

