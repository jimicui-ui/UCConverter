namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using Xunit;

public class ConvertControllerEdgeCasesTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILogger<ConvertController>> _mockLogger;
    private readonly ConvertController _controller;

    public ConvertControllerEdgeCasesTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLogger = new Mock<ILogger<ConvertController>>();
        _controller = new ConvertController(_mockService.Object, _mockLogger.Object);
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
}

