using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Domain.Enums.Nutrition;

namespace JacksFitnessApp.Application.Commands.Nutrition;

public record AddMealCommand(
    int NutritionLogId,
    string Name,
    MealType Type
) : IRequest<MealDto>;
