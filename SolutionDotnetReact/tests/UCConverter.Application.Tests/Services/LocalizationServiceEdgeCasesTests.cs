namespace UCConverter.Application.Tests.Services;

using Microsoft.Extensions.Localization;
using Moq;
using UCConverter.Application.Resources;
using UCConverter.Application.Services;
using Xunit;

/// <summary>
/// Edge case tests for LocalizationService to improve coverage
/// </summary>
public class LocalizationServiceEdgeCasesTests
{
    private readonly Mock<IStringLocalizer<SharedResources>> _mockLocalizer;
    private readonly LocalizationService _service;

    public LocalizationServiceEdgeCasesTests()
    {
        _mockLocalizer = new Mock<IStringLocalizer<SharedResources>>();
        _service = new LocalizationService(_mockLocalizer.Object);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenCategoryNameIsEmpty_ReturnsEmpty()
    {
        // Arrange
        var categoryName = "";

        // Act
        var result = _service.GetCategoryDisplayName(categoryName);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenCategoryNameIsNull_ReturnsNull()
    {
        // Arrange
        string? categoryName = null;

        // Act
        var result = _service.GetCategoryDisplayName(categoryName!);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenResourceNotFound_ReturnsCapitalizedFallback()
    {
        // Arrange
        var categoryName = "testcategory";
        var localizedString = new LocalizedString($"Category_{categoryName}", $"Category_{categoryName}", resourceNotFound: true);
        _mockLocalizer.Setup(l => l[$"Category_{categoryName}"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName(categoryName);

        // Assert
        Assert.Equal("Testcategory", result); // First letter capitalized
    }

    [Fact]
    public void GetCategoryDisplayName_WhenResourceNotFoundAndSingleChar_ReturnsCapitalized()
    {
        // Arrange
        var categoryName = "t";
        var localizedString = new LocalizedString("Category_t", "Category_t", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Category_t"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName(categoryName);

        // Assert
        Assert.Equal("T", result);
    }

    [Fact]
    public void GetCategoryDisplayName_WhenResourceFound_ReturnsLocalizedValue()
    {
        // Arrange
        var categoryName = "length";
        var localizedString = new LocalizedString("Category_length", "长度", resourceNotFound: false);
        _mockLocalizer.Setup(l => l["Category_length"]).Returns(localizedString);

        // Act
        var result = _service.GetCategoryDisplayName(categoryName);

        // Assert
        Assert.Equal("长度", result);
    }

    [Fact]
    public void GetUnitDisplayName_WhenResourceNotFound_ReturnsDefaultName()
    {
        // Arrange
        var categoryName = "length";
        var unitSymbol = "m";
        var defaultName = "Meter";
        var localizedString = new LocalizedString("Unit_length_m", "Unit_length_m", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Unit_length_m"]).Returns(localizedString);

        // Act
        var result = _service.GetUnitDisplayName(categoryName, unitSymbol, defaultName);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetUnitDisplayName_WhenResourceFound_ReturnsLocalizedValue()
    {
        // Arrange
        var categoryName = "length";
        var unitSymbol = "m";
        var defaultName = "Meter";
        var localizedString = new LocalizedString("Unit_length_m", "米", resourceNotFound: false);
        _mockLocalizer.Setup(l => l["Unit_length_m"]).Returns(localizedString);

        // Act
        var result = _service.GetUnitDisplayName(categoryName, unitSymbol, defaultName);

        // Assert
        Assert.Equal("米", result);
    }

    [Fact]
    public void GetString_WhenArgsProvided_FormatsString()
    {
        // Arrange
        var key = "TestKey";
        var localizedString = new LocalizedString(key, "Hello {0}", resourceNotFound: false);
        _mockLocalizer.Setup(l => l[key]).Returns(localizedString);

        // Act
        var result = _service.GetString(key, "World");

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void GetString_WhenNoArgs_ReturnsValue()
    {
        // Arrange
        var key = "TestKey";
        var localizedString = new LocalizedString(key, "Hello", resourceNotFound: false);
        _mockLocalizer.Setup(l => l[key]).Returns(localizedString);

        // Act
        var result = _service.GetString(key);

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void GetString_WhenMultipleArgs_FormatsString()
    {
        // Arrange
        var key = "TestKey";
        var localizedString = new LocalizedString(key, "Hello {0}, you have {1} messages", resourceNotFound: false);
        _mockLocalizer.Setup(l => l[key]).Returns(localizedString);

        // Act
        var result = _service.GetString(key, "John", 5);

        // Assert
        Assert.Equal("Hello John, you have 5 messages", result);
    }

    [Fact]
    public void GetErrorMessage_WhenResourceFound_ReturnsLocalizedError()
    {
        // Arrange
        var errorKey = "CategoryNotFound";
        var localizedString = new LocalizedString("Error_CategoryNotFound", "类别未找到", resourceNotFound: false);
        _mockLocalizer.Setup(l => l["Error_CategoryNotFound"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage(errorKey, "test");

        // Assert
        Assert.Equal("类别未找到", result);
    }

    [Fact]
    public void GetErrorMessage_WhenResourceFoundWithArgs_FormatsError()
    {
        // Arrange
        var errorKey = "CategoryNotFound";
        var localizedString = new LocalizedString("Error_CategoryNotFound", "Category '{0}' not found", resourceNotFound: false);
        _mockLocalizer.Setup(l => l["Error_CategoryNotFound"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage(errorKey, "test");

        // Assert
        Assert.Equal("Category 'test' not found", result);
    }

    [Fact]
    public void GetErrorMessage_WhenResourceNotFound_ReturnsDefaultError()
    {
        // Arrange
        var errorKey = "CategoryNotFound";
        var localizedString = new LocalizedString("Error_CategoryNotFound", "Error_CategoryNotFound", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_CategoryNotFound"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage(errorKey, "test");

        // Assert
        Assert.Equal("Category 'test' not found", result);
    }

    [Fact]
    public void GetErrorMessage_WhenUnknownErrorKey_ReturnsGenericError()
    {
        // Arrange
        var errorKey = "UnknownError";
        var localizedString = new LocalizedString("Error_UnknownError", "Error_UnknownError", resourceNotFound: true);
        _mockLocalizer.Setup(l => l["Error_UnknownError"]).Returns(localizedString);

        // Act
        var result = _service.GetErrorMessage(errorKey);

        // Assert
        Assert.Equal("An error occurred", result);
    }

    [Fact]
    public void GetErrorMessage_WhenAllErrorKeys_ReturnsCorrectDefaults()
    {
        // Arrange & Act & Assert
        var testCases = new[]
        {
            ("CategoryNotFound", new object[] { "test" }, "Category 'test' not found"),
            ("UnitNotFound", new object[] { "m" }, "Unit 'm' not found"),
            ("InvalidConversion", new object[0], "Invalid conversion"),
            ("InvalidInput", new object[0], "Invalid input"),
            ("RequestBodyRequired", new object[0], "Request body is required"),
            ("CategoryRequired", new object[0], "Category is required"),
            ("FromUnitRequired", new object[0], "FromUnit is required"),
            ("ToUnitRequired", new object[0], "ToUnit is required"),
            ("InternalServerError", new object[0], "An error occurred while processing the request"),
            ("InternalServerErrorConversion", new object[0], "An error occurred while performing the conversion"),
            ("InternalServerErrorCategories", new object[0], "An error occurred while retrieving categories"),
            ("InternalServerErrorUnits", new object[0], "An error occurred while retrieving units")
        };

        foreach (var (key, args, expected) in testCases)
        {
            var localizedString = new LocalizedString($"Error_{key}", $"Error_{key}", resourceNotFound: true);
            _mockLocalizer.Setup(l => l[$"Error_{key}"]).Returns(localizedString);

            var result = _service.GetErrorMessage(key, args);
            Assert.Equal(expected, result);
        }
    }
}

