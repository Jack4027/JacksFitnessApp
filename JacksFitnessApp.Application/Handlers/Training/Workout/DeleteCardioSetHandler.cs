using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class DeleteCardioSetHandler : IRequestHandler<DeleteCardioSetCommand>
{
    private readonly IWorkoutRepository _repository;

    public DeleteCardioSetHandler(IWorkoutRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCardioSetCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteCardioSetAsync(request.Id);
    }
}