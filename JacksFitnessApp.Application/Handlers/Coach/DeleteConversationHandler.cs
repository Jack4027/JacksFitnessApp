using JacksFitnessApp.Application.Commands.Coach;
using JacksFitnessApp.Domain.Interfaces.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Coach;

public class DeleteConversationHandler : IRequestHandler<DeleteConversationCommand>
{
    private readonly ICoachRepository _repository;

    public DeleteConversationHandler(ICoachRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _repository.GetConversationByIdAsync(request.ConversationId)
            ?? throw new KeyNotFoundException($"Conversation {request.ConversationId} not found");

        if (conversation.UserId != request.UserId)
            throw new UnauthorizedAccessException("You do not own this conversation");

        await _repository.DeleteConversationAsync(request.ConversationId);
    }
}