using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Metrics;
using JacksFitnessApp.Application.Queries.Metrics;
using JacksFitnessApp.Domain.Interfaces.Metrics;

namespace JacksFitnessApp.Application.Handlers.Metrics;

public class GetBodyMetricsHandler : IRequestHandler<GetBodyMetricsQuery, IEnumerable<BodyMetricDto>>
{
    private readonly IBodyMetricRepository _repository;
    private readonly IMapper _mapper;

    public GetBodyMetricsHandler(IBodyMetricRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BodyMetricDto>> Handle(GetBodyMetricsQuery request, CancellationToken cancellationToken)
    {
        var metrics = await _repository.GetByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<BodyMetricDto>>(metrics);
    }
}
