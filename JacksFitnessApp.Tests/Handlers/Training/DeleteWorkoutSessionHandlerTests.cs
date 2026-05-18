using FluentAssertions;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Handlers.Training.Workout;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class DeleteWorkoutSessionHandlerTests
{
    private Mock<IWorkoutRepository> _repositoryMock;
    private DeleteWorkoutSessionHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IWorkoutRepository>();
        _handler = new DeleteWorkoutSessionHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenSessionExistsAndUserOwnsIt_DeletesSession()
    {
        var session = new WorkoutSession
        {
            Id = 1,
            UserId = "user-123",
            Date = DateTime.UtcNow,
            Sets = new List<WorkoutSet>(),
            CardioSets = new List<CardioSet>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(1))
            .ReturnsAsync(session);

        _repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        await _handler.Handle(
            new DeleteWorkoutSessionCommand(1, "user-123"), CancellationToken.None);

        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Test]
    public async Task Handle_WhenSessionNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(999))
            .ReturnsAsync((WorkoutSession?)null);

        var act = async () => await _handler.Handle(
            new DeleteWorkoutSessionCommand(999, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenUserDoesNotOwnSession_ThrowsUnauthorizedAccessException()
    {
        var session = new WorkoutSession
        {
            Id = 1,
            UserId = "other-user",
            Date = DateTime.UtcNow,
            Sets = new List<WorkoutSet>(),
            CardioSets = new List<CardioSet>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(1))
            .ReturnsAsync(session);

        var act = async () => await _handler.Handle(
            new DeleteWorkoutSessionCommand(1, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task Handle_WhenUserDoesNotOwnSession_DoesNotCallDeleteAsync()
    {
        var session = new WorkoutSession
        {
            Id = 1,
            UserId = "other-user",
            Date = DateTime.UtcNow,
            Sets = new List<WorkoutSet>(),
            CardioSets = new List<CardioSet>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithSetsAsync(1))
            .ReturnsAsync(session);

        try
        {
            await _handler.Handle(
                new DeleteWorkoutSessionCommand(1, "user-123"), CancellationToken.None);
        }
        catch (UnauthorizedAccessException) { }

        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}