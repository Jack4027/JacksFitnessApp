using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;

namespace JacksFitnessApp.Application.Commands.Nutrition;

public record AddMealItemCommand(
    int MealId,
    int FoodItemId,
    decimal QuantityGrams
) : IRequest<MealItemDto>;
