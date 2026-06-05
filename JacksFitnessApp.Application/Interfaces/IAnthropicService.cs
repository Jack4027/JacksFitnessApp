using JacksFitnessApp.Application.DTOs.Coach;

namespace JacksFitnessApp.Application.Interfaces;

public interface IAnthropicService
{
    Task<string> GetCoachResponseAsync(
        string userMessage,
        List<CoachMessageDto> history,
        UserContextDto userContext);
}