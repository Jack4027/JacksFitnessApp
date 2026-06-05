using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Training.Workout;

public class DeleteWorkoutSetHandler : IRequestHandler<DeleteWorkoutSetCommand>
{
    private readonly IWorkoutRepository _repository;

    public DeleteWorkoutSetHandler(IWorkoutRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteWorkoutSetCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteSetAsync(request.Id);
    }
}
