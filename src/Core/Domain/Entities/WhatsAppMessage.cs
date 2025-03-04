namespace Core.Domain.Entities
{
    public class WhatsAppMessage
    {
        public string Id { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
