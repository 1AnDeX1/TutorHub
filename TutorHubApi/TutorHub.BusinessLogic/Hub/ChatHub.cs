using TutorHub.DataAccess.Entities;
using Microsoft.AspNetCore.SignalR;
using TutorHub.DataAccess.IRepositories;
using AutoMapper;

namespace TutorHub.BusinessLogic.Hub;

public class ChatHub(
    IUnitOfWork unitOfWork,
    IMapper mapper) : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task JoinChatGroup(string chatId, string userId)
    {
        try
        {
            var chat = await unitOfWork.Chats.GetByIdAsync(int.Parse(chatId));

            if (chat == null)
            {
                throw new HubException("Chat group not found.");
            }

            bool isTeacher = chat.Teacher.UserId == userId;
            bool isStudent = chat.Student.UserId == userId;

            if (!isTeacher && !isStudent)
            {
                throw new HubException("Not authorized to join this chat group.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }
        catch (Exception ex)
        {
            throw new HubException("Failed to join chat group.", ex);
        }
    }

    public async Task SendMessageToChat(string chatId, string userId, string message)
    {
        try
        {
            var chat = await unitOfWork.Chats.GetByIdAsync(int.Parse(chatId));

            if (chat == null)
                throw new HubException("Chat not found.");

            bool isTeacher = chat.Teacher.UserId == userId;
            bool isStudent = chat.Student.UserId == userId;

            if (!isTeacher && !isStudent)
                throw new HubException("Not authorized to send messages in this chat.");

            var chatMessage = new ChatMessage
            {
                ChatId = chat.Id,
                UserId = userId,
                SenderName = isTeacher ? chat.Teacher.User?.UserName ?? "Teacher" : chat.Student.User?.UserName ?? "Student",
                SenderRole = isTeacher ? "Teacher" : "Student",
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await unitOfWork.ChatMessages.AddAsync(chatMessage);
            await unitOfWork.SaveAsync();

            await Clients.Group(chatId).SendAsync("ReceiveMessage", new
            {
                chatMessage.ChatId,
                chatMessage.UserId,
                chatMessage.SenderName,
                chatMessage.SenderRole,
                chatMessage.Message,
                chatMessage.CreatedAt
            });
        }
        catch (Exception)
        {
            throw new HubException("Failed to send message.");
        }
    }
}
