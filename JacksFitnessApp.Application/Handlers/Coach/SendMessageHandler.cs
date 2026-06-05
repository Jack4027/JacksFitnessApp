using JacksFitnessApp.Application.Commands.Coach;
using JacksFitnessApp.Application.DTOs.Coach;
using JacksFitnessApp.Application.Interfaces;
using JacksFitnessApp.Domain.Entities.Coach;
using JacksFitnessApp.Domain.Interfaces.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Coach;

public class SendMessageHandler : IRequestHandler<SendMessageCommand, SendMessageResponseDto>
{
    private readonly ICoachRepository _coachRepository;
    private readonly IAnthropicService _anthropicService;
    private readonly IUserContextService _userContextService;

    public SendMessageHandler(
        ICoachRepository coachRepository,
        IAnthropicService anthropicService,
        IUserContextService userContextService)
    {
        _coachRepository = coachRepository;
        _anthropicService = anthropicService;
        _userContextService = userContextService;
    }

    public async Task<SendMessageResponseDto> Handle(
        SendMessageCommand request, CancellationToken cancellationToken)
    {
        // Step 1 — verify conversation exists and user owns it
        var conversation = await _coachRepository.GetConversationByIdAsync(request.ConversationId)
            ?? throw new KeyNotFoundException($"Conversation {request.ConversationId} not found");

        if (conversation.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this conversation");

        // Step 2 — save the user message
        var userMessage = new CoachMessage
        {
            ConversationId = request.ConversationId,
            Role = "user",
            Content = request.Message
        };

        await _coachRepository.AddMessageAsync(userMessage);

        // Step 3 — build history from existing messages excluding the one we just saved
        var history = conversation.Messages
            .Select(m => new CoachMessageDto
            {
                Id = m.Id,
                Role = m.Role,
                Content = m.Content,
                CreatedAt = m.CreatedAt
            })
            .ToList();

        // Step 4 — fetch user fitness context
        var userContext = await _userContextService.GetUserContextAsync(request.UserId);

        // Step 5 — call Anthropic API
        var response = await _anthropicService.GetCoachResponseAsync(
            request.Message,
            history,
            userContext);

        // Step 6 — save assistant response
        var assistantMessage = new CoachMessage
        {
            ConversationId = request.ConversationId,
            Role = "assistant",
            Content = response
        };

        await _coachRepository.AddMessageAsync(assistantMessage);

        // Step 7 — update title if this is the first message
        if (conversation.Messages.Count == 0)
        {
            var title = request.Message.Length <= 50
                ? request.Message
                : request.Message[..50] + "...";

            await _coachRepository.UpdateConversationTitleAsync(
                request.ConversationId, title);
        }

        return new SendMessageResponseDto
        {
            Response = response,
            ConversationId = request.ConversationId
        };
    }
}