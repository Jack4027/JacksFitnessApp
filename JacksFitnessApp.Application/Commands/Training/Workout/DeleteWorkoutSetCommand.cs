using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record DeleteWorkoutSetCommand(int Id, string UserId) : IRequest;
