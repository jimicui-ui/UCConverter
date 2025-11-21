namespace UCConverter.Api.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UCConverter.Api.Controllers;
using UCConverter.Application.DTOs;
using UCConverter.Application.Interfaces;
using Xunit;

public class CategoriesControllerEdgeCasesTests
{
    private readonly Mock<IUnitConverterService> _mockService;
    private readonly Mock<ILogger<CategoriesController>> _mockLogger;
    private readonly CategoriesController _controller;

    public CategoriesControllerEdgeCasesTests()
    {
        _mockService = new Mock<IUnitConverterService>();
        _mockLogger = new Mock<ILogger<CategoriesController>>();
        _controller = new CategoriesController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCategories_WhenEmpty_ReturnsOkWithEmptyList()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllCategoriesAsync())
            .ReturnsAsync(Enumerable.Empty<CategoryDto>());

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
        Assert.Empty(returnedCategories);
    }

    [Fact]
    public async Task GetUnitsByCategory_WhenCategoryNameIsWhitespace_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetUnitsByCategoryAsync("   "))
            .ReturnsAsync(Enumerable.Empty<UnitDto>());

        // Act
        var result = await _controller.GetUnitsByCategory("   ");

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.NotNull(notFoundResult.Value);
    }
}

