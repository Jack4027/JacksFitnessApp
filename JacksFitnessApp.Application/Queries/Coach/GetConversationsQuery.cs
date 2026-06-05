using JacksFitnessApp.Application.DTOs.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Coach;

public record GetConversationsQuery(string UserId)
    : IRequest<IEnumerable<CoachConversationSummaryDto>>;
