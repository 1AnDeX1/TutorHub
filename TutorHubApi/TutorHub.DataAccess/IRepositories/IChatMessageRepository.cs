using TutorHub.DataAccess.Entities;

namespace TutorHub.DataAccess.IRepositories;

public interface IChatMessageRepository
{
    Task<ChatMessage> AddAsync(ChatMessage message);
    Task<IEnumerable<ChatMessage>> GetMessagesByChatIdAsync(int chatId);
}
