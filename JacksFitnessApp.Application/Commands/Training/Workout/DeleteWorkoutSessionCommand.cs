using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record DeleteWorkoutSessionCommand(int Id, string UserId) : IRequest;
