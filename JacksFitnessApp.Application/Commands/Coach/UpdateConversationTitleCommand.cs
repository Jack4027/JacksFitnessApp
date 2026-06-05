using MediatR;

namespace JacksFitnessApp.Application.Commands.Coach;

public record UpdateConversationTitleCommand(int ConversationId, string UserId, string Title) : IRequest;