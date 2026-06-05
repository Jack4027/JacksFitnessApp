using JacksFitnessApp.Application.DTOs.Coach;

namespace JacksFitnessApp.Application.Interfaces;

public interface IUserContextService
{
    Task<UserContextDto> GetUserContextAsync(string userId);
}