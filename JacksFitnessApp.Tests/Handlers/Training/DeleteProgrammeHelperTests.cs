using FluentAssertions;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Handlers.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class DeleteProgrammeHandlerTests
{
    private Mock<IProgrammeRepository> _repositoryMock;
    private DeleteProgrammeHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IProgrammeRepository>();
        _handler = new DeleteProgrammeHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenProgrammeExistsAndUserOwnsIt_DeletesProgramme()
    {
        var programme = new Programme
        {
            Id = 1,
            UserId = "user-123",
            Name = "Test",
            Goal = ProgrammeGoal.Strength,
            DurationWeeks = 8,
            Weeks = new List<ProgrammeWeek>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(programme);

        _repositoryMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        await _handler.Handle(
            new DeleteProgrammeCommand(1, "user-123"), CancellationToken.None);

        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Test]
    public async Task Handle_WhenProgrammeNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(999))
            .ReturnsAsync((Programme?)null);

        var act = async () => await _handler.Handle(
            new DeleteProgrammeCommand(999, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenUserDoesNotOwnProgramme_ThrowsUnauthorizedAccessException()
    {
        var programme = new Programme
        {
            Id = 1,
            UserId = "other-user",
            Name = "Test",
            Goal = ProgrammeGoal.Strength,
            DurationWeeks = 8,
            Weeks = new List<ProgrammeWeek>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(programme);

        var act = async () => await _handler.Handle(
            new DeleteProgrammeCommand(1, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Test]
    public async Task Handle_WhenUserDoesNotOwnProgramme_DoesNotCallDeleteAsync()
    {
        var programme = new Programme
        {
            Id = 1,
            UserId = "other-user",
            Name = "Test",
            Goal = ProgrammeGoal.Strength,
            DurationWeeks = 8,
            Weeks = new List<ProgrammeWeek>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(programme);

        try
        {
            await _handler.Handle(
                new DeleteProgrammeCommand(1, "user-123"), CancellationToken.None);
        }
        catch (UnauthorizedAccessException) { }

        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}