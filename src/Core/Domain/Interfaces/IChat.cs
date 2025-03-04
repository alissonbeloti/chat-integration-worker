using Core.Domain.Entities;

namespace Core.Domain.Interfaces;
public interface IChat
{
    Task<bool> SendMessage(string recipient, string message);
    Task<IEnumerable<Message>> GetMessages(string conversationId);
    Task<bool> Connect();
    Task<bool> Disconnect();
} 