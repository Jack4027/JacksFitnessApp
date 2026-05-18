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
public class GetExercisesHandlerTests
{
    private Mock<IExerciseRepository> _repositoryMock;
    private IMapper _mapper;
    private GetExercisesHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IExerciseRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new GetExercisesHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_WhenNoMuscleGroupFilter_CallsGetAllAsync()
    {
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Exercise>());

        var query = new GetExercisesQuery(null);

        await _handler.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        _repositoryMock.Verify(r => r.GetByMuscleGroupAsync(It.IsAny<MuscleGroup>()), Times.Never);
    }

    [Test]
    public async Task Handle_WhenMuscleGroupProvided_CallsGetByMuscleGroupAsync()
    {
        _repositoryMock
            .Setup(r => r.GetByMuscleGroupAsync(MuscleGroup.Chest))
            .ReturnsAsync(new List<Exercise>());

        var query = new GetExercisesQuery(MuscleGroup.Chest);

        await _handler.Handle(query, CancellationToken.None);

        _repositoryMock.Verify(r => r.GetByMuscleGroupAsync(MuscleGroup.Chest), Times.Once);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Never);
    }

    [Test]
    public async Task Handle_ReturnsCorrectNumberOfExercises()
    {
        var exercises = new List<Exercise>
        {
            Exercise.CreateGlobal("Bench Press", "Barbell bench press", ExerciseType.Strength,
                MuscleGroup.Chest, MuscleGroup.Triceps, ExerciseCategory.Compound, EquipmentType.Barbell),
            Exercise.CreateGlobal("Squat", "Barbell squat", ExerciseType.Strength,
                MuscleGroup.Quadriceps, MuscleGroup.Glutes, ExerciseCategory.Compound, EquipmentType.Barbell)
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(exercises);

        var query = new GetExercisesQuery(null);
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task Handle_MapsExerciseFieldsCorrectly()
    {
        var exercises = new List<Exercise>
        {
            Exercise.CreateGlobal("Bench Press", "Barbell bench press", ExerciseType.Strength,
                MuscleGroup.Chest, MuscleGroup.Triceps, ExerciseCategory.Compound, EquipmentType.Barbell)
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(exercises);

        var query = new GetExercisesQuery(null);
        var result = await _handler.Handle(query, CancellationToken.None);
        var first = result.First();

        first.Name.Should().Be("Bench Press");
        first.Type.Should().Be(ExerciseType.Strength);
        first.PrimaryMuscleGroup.Should().Be(MuscleGroup.Chest);
        first.IsCustom.Should().BeFalse();
    }
}