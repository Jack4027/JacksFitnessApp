using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Metrics;
using JacksFitnessApp.Domain.Entities.Metrics;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using JacksFitnessApp.Application.Commands.Metrics;

namespace JacksFitnessApp.Application.Handlers.Metrics;

public class CreateBodyMetricHandler : IRequestHandler<CreateBodyMetricCommand, BodyMetricDto>
{
    private readonly IBodyMetricRepository _repository;
    private readonly IMapper _mapper;

    public CreateBodyMetricHandler(IBodyMetricRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BodyMetricDto> Handle(CreateBodyMetricCommand request, CancellationToken cancellationToken)
    {
        var metric = new BodyMetric
        {
            UserId = request.UserId,
            Date = request.Date,
            WeightKg = request.WeightKg,
            BodyFatPercentage = request.BodyFatPercentage,
            MuscleMassKg = request.MuscleMassKg,
            ChestCm = request.ChestCm,
            WaistCm = request.WaistCm,
            HipsCm = request.HipsCm,
            BicepCm = request.BicepCm,
            ThighCm = request.ThighCm,
            Notes = request.Notes
        };

        var created = await _repository.AddAsync(metric);
        return _mapper.Map<BodyMetricDto>(created);
    }
}
