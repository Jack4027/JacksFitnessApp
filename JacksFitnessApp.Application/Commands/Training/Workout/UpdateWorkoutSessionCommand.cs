using JacksFitnessApp.Application.DTOs.Training.Workout;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record UpdateWorkoutSessionCommand(
    int Id,
    int? DurationMinutes,
    string Notes,
    string UserId
) : IRequest<WorkoutSessionDto>;
