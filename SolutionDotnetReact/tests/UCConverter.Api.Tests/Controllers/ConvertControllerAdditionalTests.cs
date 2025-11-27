namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;
using Xunit;

public class ConvertControllerAdditionalTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<ConvertController>> _mockLogger;
    private readonly ConvertController _controller;

    public ConvertControllerAdditionalTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<ConvertController>>();
        _controller = new ConvertController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);
    }

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
        _mockLocalizationService.Setup(s => s.GetErrorMessage("CategoryRequired")).Returns("Category is required");

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("CategoryRequired"), Times.Once);
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
        _mockLocalizationService.Setup(s => s.GetErrorMessage("FromUnitRequired")).Returns("FromUnit is required");

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("FromUnitRequired"), Times.Once);
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
        _mockLocalizationService.Setup(s => s.GetErrorMessage("ToUnitRequired")).Returns("ToUnit is required");

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("ToUnitRequired"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenInvalidConversionException_ReturnsBadRequest()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "kg",
            Value = 10.0
        };
        _mockService.Setup(s => s.ConvertAsync(It.IsAny<ConvertRequestDto>()))
            .ThrowsAsync(new InvalidConversionException("m", "kg"));
        _mockLocalizationService.Setup(s => s.GetErrorMessage("InvalidConversion")).Returns("Invalid conversion");

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("InvalidConversion"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenGenericException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };
        _mockService.Setup(s => s.ConvertAsync(It.IsAny<ConvertRequestDto>()))
            .ThrowsAsync(new Exception("Unexpected error"));
        _mockLocalizationService.Setup(s => s.GetErrorMessage("InternalServerErrorConversion")).Returns("An error occurred while performing the conversion");

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("InternalServerErrorConversion"), Times.Once);
    }

    [Fact]
    public async Task Convert_WhenRequestIsNull_ReturnsBadRequestWithCorrectMessage()
    {
        // Arrange
        _mockLocalizationService.Setup(s => s.GetErrorMessage("RequestBodyRequired")).Returns("Request body is required");

        // Act
        var result = await _controller.Convert(null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        _mockLocalizationService.Verify(s => s.GetErrorMessage("RequestBodyRequired"), Times.Once);
    }
}

