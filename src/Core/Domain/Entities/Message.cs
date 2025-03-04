
using Core.Domain.Enum;

namespace Core.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public MessageStatus Status { get; set; }
    public MessagePlatform Platform { get; set; }
}

public enum MessageStatus
{
    Pending,
    Sent,
    Delivered,
    Failed
} 