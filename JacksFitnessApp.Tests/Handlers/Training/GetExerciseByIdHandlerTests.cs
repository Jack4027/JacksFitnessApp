using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Handlers.Training.Exercise;
using JacksFitnessApp.Application.Queries.Training.Exercise;
using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Enums.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Training;

[TestFixture]
public class GetExerciseByIdHandlerTests
{
    private Mock<IExerciseRepository> _repositoryMock;
    private IMapper _mapper;
    private GetExerciseByIdHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IExerciseRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new GetExerciseByIdHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_WhenExerciseExists_ReturnsMappedDto()
    {
        var exercise = Exercise.CreateGlobal("Bench Press", "Barbell bench press",
            ExerciseType.Strength, MuscleGroup.Chest, null,
            ExerciseCategory.Compound, EquipmentType.Barbell);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(exercise);

        var result = await _handler.Handle(new GetExerciseByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Bench Press");
    }

    [Test]
    public async Task Handle_WhenExerciseNotFound_ThrowsKeyNotFoundException()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Exercise?)null);

        var act = async () => await _handler.Handle(
            new GetExerciseByIdQuery(999), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}