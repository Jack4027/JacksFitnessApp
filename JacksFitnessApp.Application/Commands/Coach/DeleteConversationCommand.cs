using MediatR;

namespace JacksFitnessApp.Application.Commands.Coach;

public record DeleteConversationCommand(int ConversationId, string UserId)
    : IRequest;