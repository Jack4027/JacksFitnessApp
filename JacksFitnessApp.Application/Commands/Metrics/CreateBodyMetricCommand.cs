using MediatR;
using JacksFitnessApp.Application.DTOs.Metrics;

namespace JacksFitnessApp.Application.Commands.Metrics;

public record CreateBodyMetricCommand(
    string UserId,
    DateOnly Date,
    decimal? WeightKg,
    decimal? BodyFatPercentage,
    decimal? MuscleMassKg,
    decimal? ChestCm,
    decimal? WaistCm,
    decimal? HipsCm,
    decimal? BicepCm,
    decimal? ThighCm,
    string Notes
) : IRequest<BodyMetricDto>;
