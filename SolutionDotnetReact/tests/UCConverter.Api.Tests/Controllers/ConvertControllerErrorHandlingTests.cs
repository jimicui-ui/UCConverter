namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;
using Xunit;

public class ConvertControllerErrorHandlingTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<ConvertController>> _mockLogger;
    private readonly ConvertController _controller;

    public ConvertControllerErrorHandlingTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<ConvertController>>();
        _controller = new ConvertController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Convert_WhenRequestIsNull_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.Convert(null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenCategoryNotFoundException_ReturnsNotFound()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "nonexistent",
            FromUnit = "m",
            ToUnit = "km",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new CategoryNotFoundException("nonexistent"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task Convert_WhenUnitNotFoundException_ReturnsNotFound()
    {
        // Arrange
        var request = new ConvertRequestDto
        {
            Category = "length",
            FromUnit = "nonexistent",
            ToUnit = "km",
            Value = 10.0
        };

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new UnitNotFoundException("nonexistent"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
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

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new InvalidConversionException("m", "kg", "length"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Convert_WhenGeneralException_ReturnsInternalServerError()
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
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}

