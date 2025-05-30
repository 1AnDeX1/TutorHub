using System;
using System.Collections.Generic;
namespace TutorHub.DataAccess.Entities;

public class Chat
{
    public int Id { get; set; }

    public int TeacherId { get; set; }
    
    public int StudentId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Teacher Teacher { get; set; } = null!;

    public Student Student { get; set; } = null!;

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}
