namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;
using Xunit;

/// <summary>
/// Comprehensive tests for ConvertController covering all scenarios
/// </summary>
public class ConvertControllerTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<ConvertController>> _mockLogger;
    private readonly ConvertController _controller;

    public ConvertControllerTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<ConvertController>>();
        _controller = new ConvertController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);

        // Setup default localization behavior
        _mockLocalizationService.Setup(l => l.GetErrorMessage(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns<string, object[]>((key, args) =>
            {
                return key switch
                {
                    "RequestBodyRequired" => "Request body is required",
                    "CategoryRequired" => "Category is required",
                    "FromUnitRequired" => "FromUnit is required",
                    "ToUnitRequired" => "ToUnit is required",
                    "CategoryNotFound" => $"Category '{args[0]}' not found",
                    "UnitNotFound" => $"Unit '{args[0]}' not found",
                    "InvalidConversion" => "Invalid conversion",
                    "InternalServerErrorConversion" => "An error occurred while performing the conversion",
                    _ => "An error occurred"
                };
            });
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WhenUnitConverterServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ConvertController(null!, _mockLocalizationService.Object, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WhenLocalizationServiceIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ConvertController(_mockService.Object, null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ConvertController(_mockService.Object, _mockLocalizationService.Object, null!));
    }

    #endregion

    #region Validation Tests

    [Fact]
    public async Task Convert_WhenRequestIsNull_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.Convert(null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("RequestBodyRequired"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenCategoryIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenCategoryIsWhitespace_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "   ",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("CategoryRequired"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenCategoryIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = null!,
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenFromUnitIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenFromUnitIsWhitespace_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "   ",
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("FromUnitRequired"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenFromUnitIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = null!,
            ToUnit = "km",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenToUnitIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenToUnitIsWhitespace_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "   ",
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("ToUnitRequired"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenToUnitIsNull_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = null!,
            Value = 10.0
        };

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    #endregion

    #region Success Tests

    [Fact]
    public async Task Convert_WhenValid_ReturnsOkResult()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };

        var response = new ConvertResponseDto
        {
            Result = 1.0,
            FormattedResult = "1 km",
            Precision = 4,
            FromUnit = new UnitInfoDto { Symbol = "m", Name = "meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI" },
            ToUnit = new UnitInfoDto { Symbol = "km", Name = "kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI" }
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResponse = Assert.IsType<ConvertResponseDto>(okResult.Value);
        Assert.Equal(1.0, returnedResponse.Result);
        _mockService.Verify(s => s.ConvertAsync(request), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenAllValidationsPass_ReturnsOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };

        var response = new ConvertResponseDto
        {
            Result = 1.0,
            FormattedResult = "1 km",
            Precision = 4,
            FromUnit = new UnitInfoDto { Symbol = "m", Name = "meter" },
            ToUnit = new UnitInfoDto { Symbol = "km", Name = "kilometer" }
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Convert_WhenValueIsZero_ReturnsOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 0.0
        };

        var response = new ConvertResponseDto
        {
            Result = 0.0,
            FormattedResult = "0 km",
            Precision = 4,
            FromUnit = new UnitInfoDto { Symbol = "m", Name = "meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI" },
            ToUnit = new UnitInfoDto { Symbol = "km", Name = "kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI" }
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResponse = Assert.IsType<ConvertResponseDto>(okResult.Value);
        Assert.Equal(0.0, returnedResponse.Result);
    }

    [Fact]
    public async Task Convert_WhenValueIsNegative_ReturnsOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = -1000.0
        };

        var response = new ConvertResponseDto
        {
            Result = -1.0,
            FormattedResult = "-1 km",
            Precision = 4,
            FromUnit = new UnitInfoDto { Symbol = "m", Name = "meter", IsBaseUnit = true, IsSIUnit = true, UnitSystem = "SI" },
            ToUnit = new UnitInfoDto { Symbol = "km", Name = "kilometer", IsBaseUnit = false, IsSIUnit = true, UnitSystem = "SI" }
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResponse = Assert.IsType<ConvertResponseDto>(okResult.Value);
        Assert.Equal(-1.0, returnedResponse.Result);
    }

    [Fact]
    public async Task Convert_WhenValidRequestWithLocale_ReturnsOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0,
            Locale = "zh"
        };

        var response = new ConvertResponseDto
        {
            Result = 1.0,
            FormattedResult = "1 km",
            Precision = 4,
            FromUnit = new UnitInfoDto { Symbol = "m", Name = "meter" },
            ToUnit = new UnitInfoDto { Symbol = "km", Name = "kilometer" }
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Convert_WhenValidRequestWithFormula_ReturnsOk()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "temperature",
            FromUnit = "°C",
            ToUnit = "°F",
            Value = 25.0
        };

        var response = new ConvertResponseDto
        {
            Result = 77.0,
            FormattedResult = "77 °F",
            Precision = 4,
            Formula = "x * 9/5 + 32",
            FromUnit = new UnitInfoDto { Symbol = "°C", Name = "celsius" },
            ToUnit = new UnitInfoDto { Symbol = "°F", Name = "fahrenheit" }
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResponse = Assert.IsType<ConvertResponseDto>(okResult.Value);
        Assert.Equal(77.0, returnedResponse.Result);
        Assert.Equal("x * 9/5 + 32", returnedResponse.Formula);
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task Convert_WhenCategoryNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "nonexistent",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new CategoryNotFoundException("nonexistent");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task Convert_WhenCategoryNotFound_LogsWarningWithCategoryName()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "nonexistent",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new CategoryNotFoundException("nonexistent");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        await _controller.Convert(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Category not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Convert_WhenUnitNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "nonexistent",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new UnitNotFoundException("nonexistent");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task Convert_WhenUnitNotFound_LogsWarningWithUnitSymbol()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "nonexistent",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new UnitNotFoundException("nonexistent");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        await _controller.Convert(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unit not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Convert_WhenInvalidConversion_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "kg",
            Value = 10.0
        };

        var exception = new InvalidConversionException("m", "kg", "length");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("InvalidConversion"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenInvalidConversion_LogsWarningWithFromAndToUnits()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "kg",
            Value = 10.0
        };

        var exception = new InvalidConversionException("m", "kg", "length");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        await _controller.Convert(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Invalid conversion")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Convert_WhenException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new Exception("Test exception");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("InternalServerErrorConversion"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenGenericException_LogsErrorWithException()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        var exception = new Exception("Test exception");
        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(exception);

        // Act
        await _controller.Convert(request);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error performing conversion")),
                It.Is<Exception>(e => e == exception),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Convert_WhenNullReferenceException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new NullReferenceException("Null reference"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenArgumentException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new ArgumentException("Invalid argument"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }

    [Fact]
    public async Task Convert_WhenInvalidOperationException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 1000.0
        };

        _mockService.Setup(s => s.ConvertAsync(It.IsAny<ConvertRequestDto>()))
            .ThrowsAsync(new InvalidOperationException("Unexpected error"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    #endregion
}

