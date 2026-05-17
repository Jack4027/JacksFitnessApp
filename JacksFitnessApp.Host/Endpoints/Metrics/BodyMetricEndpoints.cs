using System.Security.Claims;
using JacksFitnessApp.Application.Commands.Metrics;
using JacksFitnessApp.Application.Queries.Metrics;
using MediatR;

namespace JacksFitnessApp.Host.Endpoints.Metrics;

public static class BodyMetricEndpoints
{
    public static void MapBodyMetricEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/metrics").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetBodyMetricsQuery(userId));
            return Results.Ok(result);
        });

        group.MapGet("/latest", async (IMediator mediator, ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetLatestBodyMetricQuery(userId));
            return result is null ? Results.NoContent() :Results.Ok(result);
        });

        group.MapPost("/", async (
            CreateBodyMetricCommand command,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(command with { UserId = userId });
            return Results.Created($"/api/metrics/{result.Id}", result);
        });

        group.MapPut("/{id}", async (
            int id,
            UpdateBodyMetricCommand command,
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
            await mediator.Send(new DeleteBodyMetricCommand(id, userId));
            return Results.NoContent();
        });
    }
}