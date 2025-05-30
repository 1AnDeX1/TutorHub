using Microsoft.EntityFrameworkCore;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;
using TutorHub.DataAccess.IRepositories;

namespace TutorHub.DataAccess.Repositories;

public class ChatRepository(ApplicationDbContext context) : IChatRepository
{
    public async Task<IEnumerable<Chat>> GetAllAsync()
    {
        return await context.Chats.AsNoTracking().ToListAsync();
    }

    public async Task<Chat?> GetByIdAsync(int id)
    {
        var chat = await context.Chats
            .AsNoTracking()
            .Include(c => c.Teacher)
            .ThenInclude(t => t.User)
            .Include(c => c.Student)
            .ThenInclude (s => s.User)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == id);

        return chat;
    }

    public async Task<IEnumerable<Chat>> GetAllByTeacherIdAsync(int teacherId)
    {
        var chat = await context.Chats
            .AsNoTracking()
            .Include(c => c.Teacher)
            .Include(c => c.Student)
            .Include(c => c.Messages)
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();

        return chat;
    }

    public async Task<IEnumerable<Chat>> GetAllByStudentIdAsync(int studentId)
    {
        var chat = await context.Chats
            .AsNoTracking()
            .Include(c => c.Messages)
            .Where(c => c.StudentId == studentId)
            .ToListAsync();

        return chat;
    }

    public async Task<Chat> AddAsync(Chat chat)
    {
        await context.Chats.AddAsync(chat);

        return chat;
    }

    public async Task DeleteByTeacherIdAsync(int teacherId)
    {
        var chats = await context.Chats
            .Include(c => c.Messages)
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();

        foreach (var chat in chats)
        {
            if (chat != null)
            {
                if (chat.Messages != null)
                    context.ChatMessages.RemoveRange(chat.Messages);

                context.Chats.Remove(chat);
            }
        }
    }

    public async Task DeleteByStudentIdAsync(int studentId)
    {
        var chats = await context.Chats
            .Include(c => c.Messages)
            .Where(c => c.StudentId == studentId)
            .ToListAsync();

        foreach(var chat in chats)
        {
            if (chat != null)
            {
                if (chat.Messages != null)
                    context.ChatMessages.RemoveRange(chat.Messages);

                context.Chats.Remove(chat);
            }
        }
    }

    public async Task DeleteAsync(int teacherId, int studentId)
    {
        var chat = await context.Chats
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.TeacherId == teacherId && c.StudentId == studentId);

        if (chat != null)
        {
            if (chat.Messages != null)
                context.ChatMessages.RemoveRange(chat.Messages);

            context.Chats.Remove(chat);
        }
    }
}
