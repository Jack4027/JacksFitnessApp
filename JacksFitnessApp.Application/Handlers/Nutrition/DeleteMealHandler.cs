using MediatR;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Application.Commands.Nutrition;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class DeleteMealHandler : IRequestHandler<DeleteMealCommand>
{
    private readonly INutritionRepository _repository;

    public DeleteMealHandler(INutritionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteMealCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteMealAsync(request.MealId);
    }
}
