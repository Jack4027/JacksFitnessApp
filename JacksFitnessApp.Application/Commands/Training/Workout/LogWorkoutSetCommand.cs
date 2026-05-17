using JacksFitnessApp.Application.DTOs.Training.Workout;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record LogWorkoutSetCommand(
    int WorkoutSessionId,
    int ExerciseId,
    int SetNumber,
    int RepsCompleted,
    decimal WeightKg,
    int? RestSeconds,
    string Notes
) : IRequest<WorkoutSetDto>;
