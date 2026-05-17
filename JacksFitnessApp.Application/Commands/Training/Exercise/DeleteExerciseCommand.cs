using MediatR;

namespace JacksFitnessApp.Application.Commands.Training;

public record DeleteExerciseCommand(int Id) : IRequest;