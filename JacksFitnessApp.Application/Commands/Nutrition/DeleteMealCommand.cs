using MediatR;

namespace JacksFitnessApp.Application.Commands.Nutrition;

public record DeleteMealCommand(int MealId) : IRequest;
