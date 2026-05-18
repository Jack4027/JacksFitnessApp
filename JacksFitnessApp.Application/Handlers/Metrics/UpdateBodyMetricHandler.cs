using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Metrics;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using JacksFitnessApp.Application.Commands.Metrics;

namespace JacksFitnessApp.Application.Handlers.Metrics;

public class UpdateBodyMetricHandler : IRequestHandler<UpdateBodyMetricCommand, BodyMetricDto>
{
    private readonly IBodyMetricRepository _repository;
    private readonly IMapper _mapper;

    public UpdateBodyMetricHandler(IBodyMetricRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BodyMetricDto> Handle(UpdateBodyMetricCommand request, CancellationToken cancellationToken)
    {
        var metric = await _repository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Body metric {request.Id} not found");

        if (metric.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this metric");

        metric.WeightKg = request.WeightKg;
        metric.BodyFatPercentage = request.BodyFatPercentage;
        metric.MuscleMassKg = request.MuscleMassKg;
        metric.ChestCm = request.ChestCm;
        metric.WaistCm = request.WaistCm;
        metric.HipsCm = request.HipsCm;
        metric.BicepCm = request.BicepCm;
        metric.ThighCm = request.ThighCm;
        metric.Notes = request.Notes;
        metric.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(metric);
        return _mapper.Map<BodyMetricDto>(metric);
    }
}
