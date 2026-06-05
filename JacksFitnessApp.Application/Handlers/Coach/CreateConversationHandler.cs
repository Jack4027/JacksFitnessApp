using JacksFitnessApp.Application.Commands.Coach;
using JacksFitnessApp.Application.DTOs.Coach;
using JacksFitnessApp.Domain.Entities.Coach;
using JacksFitnessApp.Domain.Interfaces.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Coach;

public class CreateConversationHandler : IRequestHandler<CreateConversationCommand, CoachConversationDto>
{
    private readonly ICoachRepository _repository;

    public CreateConversationHandler(ICoachRepository repository)
    {
        _repository = repository;
    }

    public async Task<CoachConversationDto> Handle(
        CreateConversationCommand request, CancellationToken cancellationToken)
    {
        var conversation = new CoachConversation
        {
            UserId = request.UserId,
            Title = "New conversation"
        };

        var created = await _repository.CreateConversationAsync(conversation);

        return new CoachConversationDto
        {
            Id = created.Id,
            Title = created.Title,
            CreatedAt = created.CreatedAt,
            UpdatedAt = created.UpdatedAt,
            Messages = new List<CoachMessageDto>()
        };
    }
}