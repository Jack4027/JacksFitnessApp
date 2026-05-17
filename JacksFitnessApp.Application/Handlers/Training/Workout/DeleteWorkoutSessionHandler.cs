using MediatR;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class DeleteWorkoutSessionHandler : IRequestHandler<DeleteWorkoutSessionCommand>
{
    private readonly IWorkoutRepository _repository;

    public DeleteWorkoutSessionHandler(IWorkoutRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteWorkoutSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _repository.GetByIdWithSetsAsync(request.Id)
            ?? throw new KeyNotFoundException($"Workout session {request.Id} not found");

        if (session.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this workout session");

        await _repository.DeleteAsync(request.Id);
    }
}