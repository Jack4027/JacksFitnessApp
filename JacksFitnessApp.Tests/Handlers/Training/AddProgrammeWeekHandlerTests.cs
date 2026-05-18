using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Handlers.Training.Programme;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class AddProgrammeWeekHandlerTests
{
    private Mock<IProgrammeRepository> _repositoryMock;
    private IMapper _mapper;
    private AddProgrammeWeekHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IProgrammeRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new AddProgrammeWeekHandler(_repositoryMock.Object, _mapper);

        _repositoryMock
            .Setup(r => r.AddWeekAsync(It.IsAny<ProgrammeWeek>()))
            .ReturnsAsync((ProgrammeWeek week) =>
            {
                week.Id = 1;
                week.Days = new List<ProgrammeDay>();
                return week;
            });
    }

    [Test]
    public async Task Handle_WhenProgrammeNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(999))
            .ReturnsAsync((Programme?)null);

        var command = new AddProgrammeWeekCommand(
            ProgrammeId: 999,
            WeekNumber: 1,
            Notes: ""
        );

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenProgrammeExists_AddsWeek()
    {
        var programme = new Programme
        {
            Id = 1,
            UserId = "user-123",
            Name = "Test Programme",
            Goal = ProgrammeGoal.Strength,
            DurationWeeks = 8,
            Weeks = new List<ProgrammeWeek>()
        };

        _repositoryMock
            .Setup(r => r.GetByIdWithDetailsAsync(1))
            .ReturnsAsync(programme);

        var command = new AddProgrammeWeekCommand(
            ProgrammeId: 1,
            WeekNumber: 1,
            Notes: "Heavy week"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.WeekNumber.Should().Be(1);
        result.Notes.Should().Be("Heavy week");
        _repositoryMock.Verify(r => r.AddWeekAsync(It.IsAny<ProgrammeWeek>()), Times.Once);
    }
}