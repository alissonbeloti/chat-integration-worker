namespace Core.Domain.Entities;

public class InstagramDirectMessage
{
    public string Id { get; set; } = string.Empty;
    public string SenderId { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}
