using AutoMapper;
using FluentAssertions;
using JacksFitnessApp.Application.Handlers.Nutrition;
using JacksFitnessApp.Application.Queries.Nutrition;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace JacksFitnessApp.Tests.Handlers.Nutrition;

[TestFixture]
public class GetNutritionLogByDateHandlerTests
{
    private Mock<INutritionRepository> _repositoryMock;
    private IMapper _mapper;
    private GetNutritionLogByDateHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<INutritionRepository>();
        _mapper = MapperHelper.CreateMapper();
        _handler = new GetNutritionLogByDateHandler(_repositoryMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_WhenLogExists_ReturnsMappedDto()
    {
        var date = new DateOnly(2026, 5, 16);
        var log = new NutritionLog
        {
            Id = 1,
            UserId = "user-123",
            Date = date,
            Meals = new List<Meal>()
        };

        _repositoryMock
            .Setup(r => r.GetByDateAsync("user-123", date))
            .ReturnsAsync(log);

        var result = await _handler.Handle(
            new GetNutritionLogByDateQuery("user-123", date), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Date.Should().Be(date);
    }

    [Test]
    public async Task Handle_WhenNoLogExists_ReturnsNull()
    {
        var date = new DateOnly(2026, 5, 16);

        _repositoryMock
            .Setup(r => r.GetByDateAsync("user-123", date))
            .ReturnsAsync((NutritionLog?)null);

        var result = await _handler.Handle(
            new GetNutritionLogByDateQuery("user-123", date), CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    public async Task Handle_NewLogHasZeroMacroTotals()
    {
        var date = new DateOnly(2026, 5, 16);
        var log = new NutritionLog
        {
            Id = 1,
            UserId = "user-123",
            Date = date,
            Meals = new List<Meal>()
        };

        _repositoryMock
            .Setup(r => r.GetByDateAsync("user-123", date))
            .ReturnsAsync(log);

        var result = await _handler.Handle(
            new GetNutritionLogByDateQuery("user-123", date), CancellationToken.None);

        result!.TotalCalories.Should().Be(0);
        result.TotalProtein.Should().Be(0);
        result.TotalCarbs.Should().Be(0);
        result.TotalFat.Should().Be(0);
    }
}