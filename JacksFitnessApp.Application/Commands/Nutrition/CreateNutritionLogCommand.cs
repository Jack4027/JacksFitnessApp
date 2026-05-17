using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;

namespace JacksFitnessApp.Application.Commands.Nutrition;

public record CreateNutritionLogCommand(
    string UserId,
    DateOnly Date,
    string Notes
) : IRequest<NutritionLogDto>;
