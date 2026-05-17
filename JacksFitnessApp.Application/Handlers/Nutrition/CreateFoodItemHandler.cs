using AutoMapper;
using MediatR;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Enums.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Application.Commands.Nutrition;

namespace JacksFitnessApp.Application.Handlers.Nutrition;

public class CreateFoodItemHandler : IRequestHandler<CreateFoodItemCommand, FoodItemDto>
{
    private readonly IFoodRepository _repository;
    private readonly IMapper _mapper;

    public CreateFoodItemHandler(IFoodRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FoodItemDto> Handle(CreateFoodItemCommand request, CancellationToken cancellationToken)
    {
        var foodItem = new FoodItem
        {
            Name = request.Name,
            Brand = request.Brand,
            Barcode = request.Barcode,
            CaloriesPer100g = request.CaloriesPer100g,
            ProteinPer100g = request.ProteinPer100g,
            CarbsPer100g = request.CarbsPer100g,
            FatPer100g = request.FatPer100g,
            FibrePer100g = request.FibrePer100g,
            Source = FoodItemSource.UserCreated
        };

        var created = await _repository.AddAsync(foodItem);
        return _mapper.Map<FoodItemDto>(created);
    }
}