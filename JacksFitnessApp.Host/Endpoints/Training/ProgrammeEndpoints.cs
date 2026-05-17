using JacksFitnessApp.Application.Commands.Training;
using JacksFitnessApp.Application.Queries.Training.Programme;
using MediatR;
using System.Security.Claims;

namespace JacksFitnessApp.Host.Endpoints.Training;

public static class ProgrammeEndpoints
{
    public static void MapProgrammeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/programmes").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetProgrammesQuery(userId));
            return Results.Ok(result);
        });

        group.MapGet("/{id}", async (int id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetProgrammeByIdQuery(id, userId));
            return Results.Ok(result);
        });

        group.MapPost("/", async (
            CreateProgrammeCommand command,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(command with { UserId = userId });
            return Results.Created($"/api/programmes/{result.Id}", result);
        });

        group.MapPut("/{id}", async (
            int id,
            UpdateProgrammeCommand command,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(command with { Id = id, UserId = userId });
            return Results.Ok(result);
        });

        group.MapDelete("/{id}", async (int id, IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await mediator.Send(new DeleteProgrammeCommand(id, userId));
            return Results.NoContent();
        });

        group.MapPost("/{programmeId}/weeks", async (
            int programmeId,
            AddProgrammeWeekCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command with { ProgrammeId = programmeId });
            return Results.Created($"/api/weeks/{result.Id}", result);
        });

        group.MapPost("/weeks/{weekId}/days", async (
            int weekId,
            AddProgrammeDayCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command with { ProgrammeWeekId = weekId });
            return Results.Created($"/api/days/{result.Id}", result);
        });

        group.MapPost("/days/{dayId}/exercises", async (
            int dayId,
            AddPlannedExerciseCommand command,
            IMediator mediator) =>
        {
            var result = await mediator.Send(command with { ProgrammeDayId = dayId });
            return Results.Created($"/api/planned-exercises/{result.Id}", result);
        });
    }
}