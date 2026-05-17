using JacksFitnessApp.Application.DTOs.Training.Workout;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record LogCardioSetCommand(
    int WorkoutSessionId,
    int ExerciseId,
    int SetNumber,
    int? DurationSeconds,
    decimal? DistanceKm,
    int? CaloriesBurned,
    int? AvgHeartRate,
    int? MaxHeartRate,
    string Notes
) : IRequest<CardioSetDto>;
