using JacksFitnessApp.Application.DTOs.Training;
using JacksFitnessApp.Application.DTOs.Training.Workout;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record CreateWorkoutSessionCommand(
    DateTime Date,
    int? DurationMinutes,
    string Notes,
    string UserId,
    int? ProgrammeDayId
) : IRequest<WorkoutSessionDto>;
