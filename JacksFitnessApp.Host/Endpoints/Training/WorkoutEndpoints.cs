using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Queries.Training.Workout;
using MediatR;
using System.Security.Claims;

namespace JacksFitnessApp.Host.Endpoints.Training;

public static class WorkoutEndpoints
{
    public static void MapWorkoutEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/workouts").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetWorkoutsQuery(userId));
            return Results.Ok(result);
        });

        group.MapGet("/{id}", async (int id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetWorkoutByIdQuery(id, userId));
            return Results.Ok(result);
        });

        group.MapPost("/", async (
            CreateWorkoutSessionCommand command,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(command with { UserId = userId });
            return Results.Created($"/api/workouts/{result.Id}", result);
        });

        group.MapDelete("/{id}", async (int id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await mediator.Send(new DeleteWorkoutSessionCommand(id, userId));
            return Results.NoContent();
        });

        group.MapPost("/{sessionId}/sets", async (
            int sessionId,
            LogWorkoutSetCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command with { WorkoutSessionId = sessionId });
            return Results.Created($"/api/sets/{result.Id}", result);
        });

        group.MapPost("/{sessionId}/cardio", async (
            int sessionId,
            LogCardioSetCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command with { WorkoutSessionId = sessionId });
            return Results.Created($"/api/cardio/{result.Id}", result);
        });

        group.MapGet("/personal-records", async (IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetPersonalRecordsQuery(userId));
            return Results.Ok(result);
        });
    }
}