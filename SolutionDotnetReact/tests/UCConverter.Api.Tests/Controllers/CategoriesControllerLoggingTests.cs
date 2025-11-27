namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using Xunit;

/// <summary>
/// Tests to verify logging behavior in CategoriesController
/// </summary>
public class CategoriesControllerLoggingTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILocalizationService> _mockLocalizationService;
    private readonly Mock<ILogger<CategoriesController>> _mockLogger;
    private readonly CategoriesController _controller;

    public CategoriesControllerLoggingTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLocalizationService = new Mock<ILocalizationService>();
        _mockLogger = new Mock<ILogger<CategoriesController>>();
        _controller = new CategoriesController(_mockService.Object, _mockLocalizationService.Object, _mockLogger.Object);

        _mockLocalizationService.Setup(l => l.GetErrorMessage(It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns<string, object[]>((key, args) =>
            {
                return key switch
                {
                    "InternalServerErrorCategories" => "An error occurred while retrieving categories",
                    "InternalServerErrorUnits" => "An error occurred while retrieving units",
                    _ => "An error occurred"
                };
            });
    }

    [Fact]
    public async Task GetCategories_WhenException_LogsError()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ThrowsAsync(exception);

        // Act
        await _controller.GetCategories();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error retrieving categories")),
                It.Is<Exception>(e => e == exception),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenException_LogsError()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("length"))
            .ThrowsAsync(exception);

        // Act
        await _controller.GetUnitsByCategory("length");

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error retrieving units for category")),
                It.Is<Exception>(e => e == exception),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

