using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record DeleteCardioSetCommand(int Id, string UserId) : IRequest;