using JacksFitnessApp.Domain.Entities.Coach;

namespace JacksFitnessApp.Domain.Interfaces.Coach;

public interface ICoachRepository
{
    Task<IEnumerable<CoachConversation>> GetConversationsByUserIdAsync(string userId);
    Task<CoachConversation?> GetConversationByIdAsync(int id);
    Task<CoachConversation> CreateConversationAsync(CoachConversation conversation);
    Task<CoachMessage> AddMessageAsync(CoachMessage message);
    Task UpdateConversationTitleAsync(int conversationId, string title);
    Task DeleteConversationAsync(int id);
}