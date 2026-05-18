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
public class AddProgrammeDayHandlerTests
{
    private Mock<IProgrammeRepository> _repositoryMock;
    private IMapper _mapper;
    private AddProgrammeDayHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IProgrammeRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new AddProgrammeDayHandler(_repositoryMock.Object, _mapper);

        _repositoryMock
            .Setup(r => r.AddDayAsync(It.IsAny<ProgrammeDay>()))
            .ReturnsAsync((ProgrammeDay day) =>
            {
                day.Id = 1;
                day.PlannedExercises = new List<PlannedExercise>();
                return day;
            });
    }

    [Test]
    public async Task Handle_WhenWeekNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetWeekByIdAsync(999))
            .ReturnsAsync((ProgrammeWeek?)null);

        var command = new AddProgrammeDayCommand(
            ProgrammeWeekId: 999,
            Name: "Push Day",
            DayOfWeek: DayOfWeek.Monday,
            OrderIndex: 1
        );

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenWeekExists_AddsDay()
    {
        var week = new ProgrammeWeek
        {
            Id = 1,
            WeekNumber = 1,
            ProgrammeId = 1,
            Days = new List<ProgrammeDay>()
        };

        _repositoryMock
            .Setup(r => r.GetWeekByIdAsync(1))
            .ReturnsAsync(week);

        var command = new AddProgrammeDayCommand(
            ProgrammeWeekId: 1,
            Name: "Push Day",
            DayOfWeek: DayOfWeek.Monday,
            OrderIndex: 1
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Push Day");
        result.OrderIndex.Should().Be(1);
        _repositoryMock.Verify(r => r.AddDayAsync(It.IsAny<ProgrammeDay>()), Times.Once);
    }
}