using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;

namespace JacksFitnessApp.Application.Queries.Nutrition;

public record GetNutritionLogsQuery(string UserId)
    : IRequest<IEnumerable<NutritionLogDto>>;
