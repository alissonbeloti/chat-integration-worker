public class Message
{
    public string Id { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public MessagePlatform Platform { get; set; }
}

public enum MessagePlatform
{
    WhatsApp,
    Instagram
} 