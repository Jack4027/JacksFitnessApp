using MediatR;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Application.Commands.Training;

namespace JacksFitnessApp.Application.Handlers.Training.Programme;

public class DeleteProgrammeHandler : IRequestHandler<DeleteProgrammeCommand>
{
    private readonly IProgrammeRepository _repository;

    public DeleteProgrammeHandler(IProgrammeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteProgrammeCommand request, CancellationToken cancellationToken)
    {
        var programme = await _repository.GetByIdWithDetailsAsync(request.Id)
            ?? throw new KeyNotFoundException($"Programme {request.Id} not found");

        if (programme.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this programme");

        await _repository.DeleteAsync(request.Id);
    }
}
