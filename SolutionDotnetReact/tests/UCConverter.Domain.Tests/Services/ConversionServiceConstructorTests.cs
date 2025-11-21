namespace UCConverter.Domain.Tests.Services;

using Moq;
using UCConverter.Domain.Interfaces;
using UCConverter.Domain.Services;
using Xunit;

public class ConversionServiceConstructorTests
{
    [Fact]
    public void Constructor_WhenRepositoryIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ConversionService(null!));
    }

    [Fact]
    public void Constructor_WhenRepositoryIsNotNull_CreatesInstance()
    {
        // Arrange
        var mockRepository = new Mock<IUnitRepository>();

        // Act
        var service = new ConversionService(mockRepository.Object);

        // Assert
        Assert.NotNull(service);
    }
}

