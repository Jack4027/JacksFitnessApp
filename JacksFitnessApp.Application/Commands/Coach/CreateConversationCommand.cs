using JacksFitnessApp.Application.DTOs.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Coach;

public record CreateConversationCommand(string UserId)
    : IRequest<CoachConversationDto>;
