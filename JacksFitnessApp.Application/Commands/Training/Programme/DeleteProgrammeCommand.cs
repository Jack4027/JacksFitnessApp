using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record DeleteProgrammeCommand(int Id, string UserId) : IRequest;
