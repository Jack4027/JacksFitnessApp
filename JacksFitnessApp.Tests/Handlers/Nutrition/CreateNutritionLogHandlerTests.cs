using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Commands.Nutrition;
using JacksFitnessApp.Application.Handlers.Nutrition;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Nutrition;

[TestFixture]
public class CreateNutritionLogHandlerTests
{
    private Mock<INutritionRepository> _repositoryMock;
    private IMapper _mapper;
    private CreateNutritionLogHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<INutritionRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new CreateNutritionLogHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_CreatesNutritionLogWithCorrectFields()
    {
        // Arrange
        var date = new DateOnly(2026, 5, 16);
        var userId = "user-123";

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<NutritionLog>()))
            .ReturnsAsync((NutritionLog log) =>
            {
                log.Id = 1;
                return log;
            });

        var command = new CreateNutritionLogCommand(
            UserId: userId,
            Date: date,
            Notes: "Test log"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Date.Should().Be(date);
        result.Notes.Should().Be("Test log");
    }

    [Test]
    public async Task Handle_CallsRepositoryAddAsync()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<NutritionLog>()))
            .ReturnsAsync((NutritionLog log) => log);

        var command = new CreateNutritionLogCommand(
            UserId: "user-123",
            Date: new DateOnly(2026, 5, 16),
            Notes: ""
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert — verify the repository was called exactly once
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<NutritionLog>()), Times.Once);
    }

    [Test]
    public async Task Handle_NewLogHasZeroMacros()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<NutritionLog>()))
            .ReturnsAsync((NutritionLog log) => log);

        var command = new CreateNutritionLogCommand(
            UserId: "user-123",
            Date: new DateOnly(2026, 5, 16),
            Notes: ""
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert — new log should have zero totals
        result.TotalCalories.Should().Be(0);
        result.TotalProtein.Should().Be(0);
        result.TotalCarbs.Should().Be(0);
        result.TotalFat.Should().Be(0);
    }
}