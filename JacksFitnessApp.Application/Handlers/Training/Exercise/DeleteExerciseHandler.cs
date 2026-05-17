using MediatR;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Exercise;

public class DeleteExerciseHandler : IRequestHandler<DeleteExerciseCommand>
{
    private readonly IExerciseRepository _repository;

    public DeleteExerciseHandler(IExerciseRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
    }
}