using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;

namespace JacksFitnessApp.Application.Queries.Nutrition;

public record GetNutritionLogByDateQuery(string UserId, DateOnly Date)
    : IRequest<NutritionLogDto?>;
