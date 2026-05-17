using JacksFitnessApp.Application.DTOs.Training.Exercise;
using JacksFitnessApp.Domain.Enums.Training;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record UpdateExerciseCommand(
    int Id,
    string Name,
    string Description,
    MuscleGroup? PrimaryMuscleGroup,
    MuscleGroup? SecondaryMuscleGroup,
    ExerciseCategory Category,
    EquipmentType Equipment
) : IRequest<ExerciseDto>;
