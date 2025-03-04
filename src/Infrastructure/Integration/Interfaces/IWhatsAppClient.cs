using Core.Domain.Entities;

namespace Infrastructure.Integration.Interfaces
{
    public interface IWhatsAppClient
    {
        Task<bool> Initialize();
        Task<bool> Disconnect();
        Task<IEnumerable<WhatsAppMessage>> GetMessages(string conversationId);
        Task<bool> SendMessage(string recipient, string message);
    }
}
