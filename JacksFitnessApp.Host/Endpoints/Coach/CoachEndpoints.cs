using System.Security.Claims;
using JacksFitnessApp.Application.Commands.Coach;
using JacksFitnessApp.Application.DTOs.Coach;
using JacksFitnessApp.Application.Queries.Coach;
using MediatR;

namespace JacksFitnessApp.Host.Endpoints;

public static class CoachEndpoints
{
    public static void MapCoachEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/coach").RequireAuthorization();

        // Get all conversations for the current user
        group.MapGet("/conversations", async (
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetConversationsQuery(userId));
            return Results.Ok(result);
        });

        // Get a single conversation with full message history
        group.MapGet("/conversations/{id}", async (
            int id,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new GetConversationByIdQuery(id, userId));
            return Results.Ok(result);
        });

        // Create a new conversation
        group.MapPost("/conversations", async (
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new CreateConversationCommand(userId));
            return Results.Created($"/api/coach/conversations/{result.Id}", result);
        });

        // Send a message in an existing conversation
        group.MapPost("/conversations/{id}/messages", async (
            int id,
            SendMessageRequestDto request,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await mediator.Send(new SendMessageCommand(id, userId, request.Message));
            return Results.Ok(result);
        });

        // Delete a conversation
        group.MapDelete("/conversations/{id}", async (
            int id,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await mediator.Send(new DeleteConversationCommand(id, userId));
            return Results.NoContent();
        });

        // Update conversation title
        group.MapPatch("/conversations/{id}", async (
            int id,
            UpdateConversationTitleRequestDto request,
            IMediator mediator,
            ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await mediator.Send(new UpdateConversationTitleCommand(id, userId, request.Title));
            return Results.NoContent();
        });
    }
}