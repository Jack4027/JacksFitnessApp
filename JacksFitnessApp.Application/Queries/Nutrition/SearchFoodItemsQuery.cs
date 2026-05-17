using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;

namespace JacksFitnessApp.Application.Queries.Nutrition;

public record SearchFoodItemsQuery(string Query)
    : IRequest<IEnumerable<FoodItemDto>>;
