using JacksFitnessApp.Application.Commands.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using MediatR;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class DeleteMealItemHandler : IRequestHandler<DeleteMealItemCommand>
{
    private readonly INutritionRepository _repository;

    public DeleteMealItemHandler(INutritionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteMealItemCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteMealItemAsync(request.Id);
    }
}