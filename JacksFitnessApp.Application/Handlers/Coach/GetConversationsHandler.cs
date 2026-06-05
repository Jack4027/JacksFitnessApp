using AutoMapper;
using JacksFitnessApp.Application.DTOs.Coach;
using JacksFitnessApp.Application.Queries.Coach;
using JacksFitnessApp.Domain.Interfaces.Coach;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Coach;

public class GetConversationsHandler : IRequestHandler<GetConversationsQuery, IEnumerable<CoachConversationSummaryDto>>
{
    private readonly ICoachRepository _repository;

    public GetConversationsHandler(ICoachRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CoachConversationSummaryDto>> Handle(
        GetConversationsQuery request, CancellationToken cancellationToken)
    {
        var conversations = await _repository.GetConversationsByUserIdAsync(request.UserId);

        return conversations.Select(c => new CoachConversationSummaryDto
        {
            Id = c.Id,
            Title = c.Title,
            UpdatedAt = c.UpdatedAt
        });
    }
}