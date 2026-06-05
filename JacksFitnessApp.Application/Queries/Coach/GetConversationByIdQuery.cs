using JacksFitnessApp.Application.DTOs.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Queries.Coach;

public record GetConversationByIdQuery(int ConversationId, string UserId)
    : IRequest<CoachConversationDto>;