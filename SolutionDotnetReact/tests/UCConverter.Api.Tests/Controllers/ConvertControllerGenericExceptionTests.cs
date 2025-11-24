namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Api.Controllers;
using Xunit;

public class ConvertControllerGenericExceptionTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<ConvertController>> _mockLogger;
    private readonly ConvertController _controller;

    public ConvertControllerGenericExceptionTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<ConvertController>>();
        _controller = new ConvertController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Convert_WhenServiceThrowsGenericException_Returns500StatusCode()
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
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Convert_WhenServiceThrowsArgumentException_Returns500StatusCode()
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
            .ThrowsAsync(new ArgumentException("Invalid argument"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}

