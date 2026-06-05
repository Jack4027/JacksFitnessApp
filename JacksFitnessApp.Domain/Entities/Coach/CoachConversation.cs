// CoachConversation.cs
using JacksFitnessApp.Domain.Common;

namespace JacksFitnessApp.Domain.Entities.Coach;

public class CoachConversation : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public ICollection<CoachMessage> Messages { get; set; } = new List<CoachMessage>();
}