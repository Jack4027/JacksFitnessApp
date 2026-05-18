using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Handlers.Training.Programme;
using JacksFitnessApp.Application.Queries.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class GetProgrammeByIdHandlerTests
{
    private Mock<IProgrammeRepository> _repositoryMock;
    private IMapper _mapper;
    private GetProgrammeByIdHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IProgrammeRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new GetProgrammeByIdHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_WhenProgrammeExistsAndUserOwnsIt_ReturnsMappedDto()
    {
        var programme = new Programme
        {
            Id = 1,
            Name = "Test Programme",
            UserId = "user-123",
            Goal = ProgrammeGoal.Strength,
            DurationWeeks = 8,
            Weeks = new List<ProgrammeWeek>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(programme);

        var result = await _handler.Handle(
            new GetProgrammeByIdQuery(1, "user-123"), CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Test Programme");
    }

    [Test]
    public async Task Handle_WhenProgrammeNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(999))
            .ReturnsAsync((Programme?)null);

        var act = async () => await _handler.Handle(
            new GetProgrammeByIdQuery(999, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenUserDoesNotOwnProgramme_ThrowsUnauthorizedAccessException()
    {
        var programme = new Programme
        {
            Id = 1,
            Name = "Someone Elses Programme",
            UserId = "other-user",
            Goal = ProgrammeGoal.Strength,
            DurationWeeks = 8,
            Weeks = new List<ProgrammeWeek>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(programme);

        var act = async () => await _handler.Handle(
            new GetProgrammeByIdQuery(1, "user-123"), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}