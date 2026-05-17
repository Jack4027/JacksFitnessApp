using MediatR;
using JacksFitnessApp.Application.DTOs.Metrics;

namespace JacksFitnessApp.Application.Queries.Metrics;

public record GetBodyMetricsQuery(string UserId)
    : IRequest<IEnumerable<BodyMetricDto>>;
