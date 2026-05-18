using FluentAssertions;
using JacksFitnessApp.Application.Commands.Nutrition;
using JacksFitnessApp.Application.Handlers.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Nutrition;

[TestFixture]
public class DeleteMealHandlerTests
{
    private Mock<INutritionRepository> _repositoryMock;
    private DeleteMealHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<INutritionRepository>();
        _handler = new DeleteMealHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenMealExists_DeletesMeal()
    {
        _repositoryMock
            .Setup(r => r.DeleteMealAsync(1))
            .Returns(Task.CompletedTask);

        await _handler.Handle(
            new DeleteMealCommand(1), CancellationToken.None);

        _repositoryMock.Verify(r => r.DeleteMealAsync(1), Times.Once);
    }

    [Test]
    public async Task Handle_WhenMealNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.DeleteMealAsync(999))
            .ThrowsAsync(new KeyNotFoundException("Meal 999 not found"));

        var act = async () => await _handler.Handle(
            new DeleteMealCommand(999), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}