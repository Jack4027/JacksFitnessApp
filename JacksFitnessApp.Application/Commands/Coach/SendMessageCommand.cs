using JacksFitnessApp.Application.DTOs.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Commands.Coach;

public record SendMessageCommand(
    int ConversationId,
    string UserId,
    string Message
) : IRequest<SendMessageResponseDto>;
