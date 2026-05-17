using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Metrics;
using JacksFitnessApp.Application.Queries.Metrics;
using JacksFitnessApp.Domain.Interfaces.Metrics;

namespace JacksFitnessApp.Application.Handlers.Metrics;

public class GetLatestBodyMetricHandler : IRequestHandler<GetLatestBodyMetricQuery, BodyMetricDto>
{
    private readonly IBodyMetricRepository _repository;
    private readonly IMapper _mapper;

    public GetLatestBodyMetricHandler(IBodyMetricRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BodyMetricDto> Handle(GetLatestBodyMetricQuery request, CancellationToken cancellationToken)
    {
        var metric = await _repository.GetLatestAsync(request.UserId);
        if (metric == null) return null;

        return  _mapper.Map<BodyMetricDto>(metric);
    }
}
