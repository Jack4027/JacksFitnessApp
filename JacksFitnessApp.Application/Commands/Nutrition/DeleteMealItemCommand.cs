using MediatR;

namespace JacksFitnessApp.Application.Commands.Nutrition;

public record DeleteMealItemCommand(int Id) : IRequest;
