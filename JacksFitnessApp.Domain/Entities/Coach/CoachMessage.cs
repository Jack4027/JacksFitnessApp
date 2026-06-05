using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Coach;

public class CoachMessage : BaseEntity
{
    public int ConversationId { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public CoachConversation Conversation { get; set; } = null!;
}
