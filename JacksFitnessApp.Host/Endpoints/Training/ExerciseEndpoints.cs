using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Queries.Training.Exercise;
using JacksFitnessApp.Domain.Enums.Training;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JacksFitnessApp.Host.Endpoints.Training;

public static class ExerciseEndpoints
{
    public static void MapExerciseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/exercises").RequireAuthorization();

        group.MapGet("/", async (
            [FromQuery] MuscleGroup? muscleGroup,
            IMediator mediator) =>
        {
            var result = await mediator.Send(new GetExercisesQuery(muscleGroup));
            return Results.Ok(result);
        });

        group.MapGet("/{id}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetExerciseByIdQuery(id));
            return Results.Ok(result);
        });

        group.MapPost("/", async (
            CreateExerciseCommand command,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(command with { UserId = userId });
            return Results.Created($"/api/exercises/{result.Id}", result);
        });

        group.MapPut("/{id}", async (
            int id,
            UpdateExerciseCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command with { Id = id });
            return Results.Ok(result);
        });

        group.MapDelete("/{id}", async (int id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteExerciseCommand(id));
            return Results.NoContent();
        });
    }
}