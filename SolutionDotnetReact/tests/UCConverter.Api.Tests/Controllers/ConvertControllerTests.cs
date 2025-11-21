namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using UCConverter.Domain.Exceptions;
using Xunit;

public class ConvertControllerTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILogger<ConvertController>> _mockLogger;
    private readonly ConvertController _controller;

    public ConvertControllerTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLogger = new Mock<ILogger<ConvertController>>();
        _controller = new ConvertController(_mockService.Object, _mockLogger.Object);
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
    }

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

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new CategoryNotFoundException("nonexistent"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
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

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new UnitNotFoundException("nonexistent"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
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

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new InvalidConversionException("m", "kg", "length"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
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

        _mockService.Setup(s => s.ConvertAsync(request))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Convert(request);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusResult.StatusCode);
    }
}

