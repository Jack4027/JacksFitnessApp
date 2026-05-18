using FluentAssertions;
using JacksFitnessApp.Application.Commands.Nutrition;
using JacksFitnessApp.Application.Handlers.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Nutrition;

[TestFixture]
public class DeleteMealItemHandlerTests
{
    private Mock<INutritionRepository> _repositoryMock;
    private DeleteMealItemHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<INutritionRepository>();
        _handler = new DeleteMealItemHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenMealItemExists_DeletesMealItem()
    {
        _repositoryMock
            .Setup(r => r.DeleteMealItemAsync(1))
            .Returns(Task.CompletedTask);

        await _handler.Handle(
            new DeleteMealItemCommand(1), CancellationToken.None);

        _repositoryMock.Verify(r => r.DeleteMealItemAsync(1), Times.Once);
    }

    [Test]
    public async Task Handle_WhenMealItemNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.DeleteMealItemAsync(999))
            .ThrowsAsync(new KeyNotFoundException("Meal item 999 not found"));

        var act = async () => await _handler.Handle(
            new DeleteMealItemCommand(999), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}