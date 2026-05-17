using System.Security.Claims;
using JacksFitnessApp.Application.Commands.Nutrition;
using JacksFitnessApp.Application.Queries.Nutrition;
using MediatR;

namespace JacksFitnessApp.Host.Endpoints.Nutrition;

public static class NutritionEndpoints
{
    public static void MapNutritionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/nutrition").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetNutritionLogsQuery(userId));
            return Results.Ok(result);
        });

        group.MapGet("/date/{date}", async (
            DateOnly date,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetNutritionLogByDateQuery(userId, date));
            return result is null ? Results.NoContent() : Results.Ok(result);
        });

        group.MapPost("/", async (
            CreateNutritionLogCommand command,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(command with { UserId = userId });
            return Results.Created($"/api/nutrition/{result.Id}", result);
        });

        group.MapPost("/{logId}/meals", async (
            int logId,
            AddMealCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command with { NutritionLogId = logId });
            return Results.Created($"/api/meals/{result.Id}", result);
        });
        group.MapDelete("/{logId}/meals/{mealId}", async (
    int logId,
    int mealId,
    IMediator mediator) =>
        {
            await mediator.Send(new DeleteMealCommand(mealId));
            return Results.NoContent();
        });

        group.MapPost("/meals/{mealId}/items", async (
            int mealId,
            AddMealItemCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command with { MealId = mealId });
            return Results.Created($"/api/meal-items/{result.Id}", result);
        });

        group.MapDelete("/meal-items/{id}", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteMealItemCommand(id));
            return Results.NoContent();
        });

        group.MapGet("/food/search", async (
            string q,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new SearchFoodItemsQuery(q));
            return Results.Ok(result);
        });

        group.MapGet("/food/barcode/{barcode}", async (
            string barcode,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetFoodItemByBarcodeQuery(barcode));
            return result is null ? Results.NoContent() : Results.Ok(result);
        });

        group.MapPost("/food", async (
            CreateFoodItemCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/food/{result.Id}", result);
        });
    }
}