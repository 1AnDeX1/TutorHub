using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.DataAccess.Repositories;

public class ChatMessageRepository(ApplicationDbContext context) : IChatMessageRepository
{
    public async Task<IEnumerable<ChatMessage>> GetMessagesByChatIdAsync(int chatId)
    {
        return await context.ChatMessages
            .Include(m => m.User)
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<ChatMessage> AddAsync(ChatMessage message)
    {
        await context.ChatMessages.AddAsync(message);
        await context.SaveChangesAsync();
        return message;
    }
}
