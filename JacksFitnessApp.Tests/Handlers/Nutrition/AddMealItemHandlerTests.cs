using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Commands.Nutrition;
using JacksFitnessApp.Application.Handlers.Nutrition;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Enums.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Nutrition;

[TestFixture]
public class AddMealItemHandlerTests
{
    private Mock<INutritionRepository> _nutritionRepositoryMock;
    private Mock<IFoodRepository> _foodRepositoryMock;
    private IMapper _mapper;
    private AddMealItemHandler _handler;

    private FoodItem _chickenBreast;

    [SetUp]
    public void SetUp()
    {
        _nutritionRepositoryMock = new Mock<INutritionRepository>();
        _foodRepositoryMock = new Mock<IFoodRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new AddMealItemHandler(
            _nutritionRepositoryMock.Object,
            _foodRepositoryMock.Object,
            _mapper);

        _chickenBreast = new FoodItem
        {
            Id = 1,
            Name = "Chicken Breast",
            CaloriesPer100g = 165,
            ProteinPer100g = 31,
            CarbsPer100g = 0,
            FatPer100g = 3.6m,
            FibrePer100g = 0,
            Source = FoodItemSource.Seeded
        };
    }

    [Test]
    public async Task Handle_WhenFoodItemNotFound_ThrowsKeyNotFoundException()
    {
        _foodRepositoryMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((FoodItem?)null);

        var command = new AddMealItemCommand(
            MealId: 1,
            FoodItemId: 999,
            QuantityGrams: 200
        );

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Handle_WhenFoodItemExists_AddsMealItem()
    {
        _foodRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(_chickenBreast);

        _nutritionRepositoryMock
            .Setup(r => r.AddMealItemAsync(It.IsAny<MealItem>()))
            .ReturnsAsync((MealItem item) =>
            {
                item.Id = 1;
                item.FoodItem = _chickenBreast;
                return item;
            });

        var command = new AddMealItemCommand(
            MealId: 1,
            FoodItemId: 1,
            QuantityGrams: 200
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.QuantityGrams.Should().Be(200);
    }

    [Test]
    public async Task Handle_MacrosCalculatedCorrectlyFromQuantity()
    {
        _foodRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(_chickenBreast);

        _nutritionRepositoryMock
            .Setup(r => r.AddMealItemAsync(It.IsAny<MealItem>()))
            .ReturnsAsync((MealItem item) =>
            {
                item.Id = 1;
                item.FoodItem = _chickenBreast;
                return item;
            });

        var command = new AddMealItemCommand(
            MealId: 1,
            FoodItemId: 1,
            QuantityGrams: 200
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        // 200g of chicken: 165 * 200/100 = 330 calories
        result.Calories.Should().Be(330);
        // 31 * 200/100 = 62g protein
        result.Protein.Should().Be(62);
        // 0 * 200/100 = 0g carbs
        result.Carbs.Should().Be(0);
    }

    [Test]
    public async Task Handle_CallsAddMealItemAsyncOnce()
    {
        _foodRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(_chickenBreast);

        _nutritionRepositoryMock
            .Setup(r => r.AddMealItemAsync(It.IsAny<MealItem>()))
            .ReturnsAsync((MealItem item) =>
            {
                item.FoodItem = _chickenBreast;
                return item;
            });

        var command = new AddMealItemCommand(
            MealId: 1,
            FoodItemId: 1,
            QuantityGrams: 100
        );

        await _handler.Handle(command, CancellationToken.None);

        _nutritionRepositoryMock.Verify(
            r => r.AddMealItemAsync(It.IsAny<MealItem>()), Times.Once);
    }
}