using MediatR;
using JacksFitnessApp.Application.DTOs.Metrics;

namespace JacksFitnessApp.Application.Queries.Metrics;

public record GetLatestBodyMetricQuery(string UserId)
    : IRequest<BodyMetricDto?>;