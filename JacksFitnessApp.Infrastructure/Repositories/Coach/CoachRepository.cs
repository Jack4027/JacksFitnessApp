using JacksFitnessApp.Domain.Entities.Coach;
using JacksFitnessApp.Domain.Interfaces.Coach;
using JacksFitnessApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JacksFitnessApp.Infrastructure.Repositories.Coach;

public class CoachRepository : ICoachRepository
{
    private readonly FitnessAppDbContext _context;

    public CoachRepository(FitnessAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CoachConversation>> GetConversationsByUserIdAsync(string userId)
    {
        return await _context.CoachConversations
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.UpdatedAt)
            .ToListAsync();
    }

    public async Task<CoachConversation?> GetConversationByIdAsync(int id)
    {
        return await _context.CoachConversations
            .Include(c => c.Messages.OrderBy(m => m.CreatedAt))
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CoachConversation> CreateConversationAsync(CoachConversation conversation)
    {
        _context.CoachConversations.Add(conversation);
        await _context.SaveChangesAsync();
        return conversation;
    }

    public async Task<CoachMessage> AddMessageAsync(CoachMessage message)
    {
        _context.CoachMessages.Add(message);

        // Update the conversation's UpdatedAt so it sorts to the top of the list
        var conversation = await _context.CoachConversations
            .FindAsync(message.ConversationId);

        if (conversation != null)
            conversation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return message;
    }

    public async Task UpdateConversationTitleAsync(int conversationId, string title)
    {
        var conversation = await _context.CoachConversations
            .FindAsync(conversationId);

        if (conversation != null)
        {
            conversation.Title = title;
            await _context.SaveChangesAsync();
        }
    }
    public async Task DeleteConversationAsync(int id)
    {
        var conversation = await _context.CoachConversations.FindAsync(id)
            ?? throw new KeyNotFoundException($"Conversation {id} not found");

        _context.CoachConversations.Remove(conversation);
        await _context.SaveChangesAsync();
    }
}