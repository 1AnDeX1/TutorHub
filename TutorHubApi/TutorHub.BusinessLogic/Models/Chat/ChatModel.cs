
namespace TutorHub.BusinessLogic.Models.Chat;

public class ChatModel
{
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public int StudentId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
