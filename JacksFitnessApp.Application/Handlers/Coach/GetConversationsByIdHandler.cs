using JacksFitnessApp.Application.DTOs.Coach;
using JacksFitnessApp.Application.Queries.Coach;
using JacksFitnessApp.Domain.Interfaces.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Coach;

public class GetConversationByIdHandler : IRequestHandler<GetConversationByIdQuery, CoachConversationDto>
{
    private readonly ICoachRepository _repository;

    public GetConversationByIdHandler(ICoachRepository repository)
    {
        _repository = repository;
    }

    public async Task<CoachConversationDto> Handle(
        GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        var conversation = await _repository.GetConversationByIdAsync(request.ConversationId)
            ?? throw new KeyNotFoundException($"Conversation {request.ConversationId} not found");

        if (conversation.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this conversation");

        return new CoachConversationDto
        {
            Id = conversation.Id,
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt,
            Messages = conversation.Messages.Select(m => new CoachMessageDto
            {
                Id = m.Id,
                Role = m.Role,
                Content = m.Content,
                CreatedAt = m.CreatedAt
            }).ToList()
        };
    }
}