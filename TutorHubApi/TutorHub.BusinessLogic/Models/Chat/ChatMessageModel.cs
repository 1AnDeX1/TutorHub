
namespace TutorHub.BusinessLogic.Models.Chat;

public class ChatMessageModel
{
    public int ChatId { get; set; }

    public string UserId { get; set; }

    public string SenderName { get; set; } = null!;

    public string SenderRole { get; set; } = null!;

    public string Message { get; set; } = null!;
}
