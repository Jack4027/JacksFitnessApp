using MediatR;

namespace JacksFitnessApp.Application.Commands.Metrics;

public record DeleteBodyMetricCommand(int Id, string UserId) : IRequest;