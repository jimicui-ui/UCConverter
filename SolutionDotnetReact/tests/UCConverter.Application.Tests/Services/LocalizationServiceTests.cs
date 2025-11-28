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

    [Fact]
    public void GetCategoryDisplayName_WhenTranslationExists_ReturnsLocalizedValue()
    {
        // Arrange
        var localizedString = new LocalizedString("Category_weight", "Weight / Mass");
        _mockLocalizer.Setup(l => l["Category_weight"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName("weight");

        // Assert
        Assert.Equal("Weight / Mass", result);
    }

    [Fact]
    public void GetUnitDisplayName_WhenTranslationExists_ReturnsLocalizedValue()
    {
        // Arrange
        var localizedString = new LocalizedString("Unit_weight_kg", "Kilogram");
        _mockLocalizer.Setup(l => l["Unit_weight_kg"]).Returns(localizedString);

        // Act
        var result = _service.GetUnitDisplayName("weight", "kg", "kilogram");

        // Assert
        Assert.Equal("Kilogram", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationExistsWithArgs_FormatsString()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_CategoryNotFound", "La catégorie '{0}' n'a pas été trouvée");
        _mockLocalizer.Setup(l => l["Error_CategoryNotFound"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("CategoryNotFound", "test");

        // Assert
        Assert.Equal("La catégorie 'test' n'a pas été trouvée", result);
    }

    [Fact]
    public void GetErrorMessage_WhenTranslationExistsWithoutArgs_ReturnsLocalizedValue()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_InvalidConversion", "Conversion invalide");
        _mockLocalizer.Setup(l => l["Error_InvalidConversion"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("InvalidConversion");

        // Assert
        Assert.Equal("Conversion invalide", result);
    }

    [Fact]
    public void GetString_WithMultipleArgs_FormatsCorrectly()
    {
        // Arrange
        var localizedString = new LocalizedString("TestKey", "Hello {0}, {1}, and {2}");
        _mockLocalizer.Setup(l => l["TestKey"]).Returns(localizedString);

        // Act
        var result = _service.GetString("TestKey", "Alice", "Bob", "Charlie");

        // Assert
        Assert.Equal("Hello Alice, Bob, and Charlie", result);
    }

    [Fact]
    public void GetErrorMessage_WithRequestBodyRequiredKey_ReturnsDefaultMessage()
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
    public void GetErrorMessage_WithCategoryRequiredKey_ReturnsDefaultMessage()
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
    public void GetErrorMessage_WithFromUnitRequiredKey_ReturnsDefaultMessage()
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
    public void GetErrorMessage_WithToUnitRequiredKey_ReturnsDefaultMessage()
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
    public void GetErrorMessage_WithInternalServerErrorKey_ReturnsDefaultMessage()
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
    public void GetErrorMessage_WithInternalServerErrorConversionKey_ReturnsDefaultMessage()
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
    public void GetErrorMessage_WithInternalServerErrorCategoriesKey_ReturnsDefaultMessage()
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
    public void GetErrorMessage_WithInternalServerErrorUnitsKey_ReturnsDefaultMessage()
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
    public void GetErrorMessage_WithUnknownErrorKey_ReturnsGenericDefaultMessage()
    {
        // Arrange
        var localizedString = new LocalizedString("Error_SomeUnknownError", "Error_SomeUnknownError", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_SomeUnknownError"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage("SomeUnknownError");

        // Assert
        Assert.Equal("An error occurred", result);
    }

    [Fact]
    public void GetErrorMessage_WithDefaultErrorKeyAndNoArgs_ReturnsGenericMessage()
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
    public void GetCategoryDisplayName_WhenCategoryNameIsWhitespace_ReturnsWhitespace()
    {
        // Arrange
        var localizedString = new LocalizedString("Category_ ", "Category_ ", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Category_ "]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName(" ");

        // Assert
        Assert.Equal(" ", result);
    }
}

