using JacksFitnessApp.Application.DTOs.Training.Programme;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record AddPlannedExerciseCommand(
    int ProgrammeDayId,
    int ExerciseId,
    int OrderIndex,
    int TargetSets,
    int TargetRepsMin,
    int TargetRepsMax,
    decimal? TargetWeight,
    string Notes
) : IRequest<PlannedExerciseDto>;