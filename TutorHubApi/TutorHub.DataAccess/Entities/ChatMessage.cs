namespace TutorHub.DataAccess.Entities;

public class ChatMessage
{
    public int Id { get; set; }

    public int ChatId { get; set; }
    
    public required string UserId { get; set; }
    
    public string SenderName { get; set; } = null!;

    public string SenderRole { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Chat Chat { get; set; } = null!;

    public User User { get; set; } = null!;
}

