using MediatR;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using JacksFitnessApp.Application.Commands.Metrics;

namespace JacksFitnessApp.Application.Handlers.Metrics;

public class DeleteBodyMetricHandler : IRequestHandler<DeleteBodyMetricCommand>
{
    private readonly IBodyMetricRepository _repository;

    public DeleteBodyMetricHandler(IBodyMetricRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteBodyMetricCommand request, CancellationToken cancellationToken)
    {
        var metric = await _repository.GetLatestAsync(request.UserId)
            ?? throw new KeyNotFoundException($"Body metric {request.Id} not found");

        if (metric.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this metric");

        await _repository.DeleteAsync(request.Id);
    }
}