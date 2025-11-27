namespace UCConverter.Application.Tests.Services;

using Microsoft.Extensions.Localization;
using Moq;
using UCConverter.Application.Resources;
using UCConverter.Application.Services;
using Xunit;

public class LocalizationServiceAdditionalTests
{
    private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
    private readonly LocalizationService _service;

    public LocalizationServiceAdditionalTests()
    {
        _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
        _service = new LocalizationService(_mockLocalizer.Object);
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
    public void GetErrorMessage_WithUnknownErrorKey_ReturnsGenericMessage()
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

    [Fact]
    public void GetUnitDisplayName_WhenCategoryNameIsNull_ReturnsDefaultName()
    {
        // Arrange
        var localizedString = new LocalizedString("Unit__m", "Unit__m", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Unit__m"]).Returns(localizedString);

        // Act
        var result = _service.GetUnitDisplayName(null!, "m", "Meter");

        // Assert
        Assert.Equal("Meter", result);
    }

    [Fact]
    public void GetUnitDisplayName_WhenCategoryNameIsEmpty_ReturnsDefaultName()
    {
        // Arrange
        var localizedString = new LocalizedString("Unit__m", "Unit__m", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Unit__m"]).Returns(localizedString);

        // Act
        var result = _service.GetUnitDisplayName("", "m", "Meter");

        // Assert
        Assert.Equal("Meter", result);
    }
}

