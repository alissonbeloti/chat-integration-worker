using Core.Domain.Entities;

using Infrastructure.Integration.Interfaces;

namespace Infrastructure.Integration
{
    public class WhatsAppClient : IWhatsAppClient
    {
        public Task<bool> Disconnect()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WhatsAppMessage>> GetMessages(string conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Initialize()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendMessage(string recipient, string message)
        {
            throw new NotImplementedException();
        }
    }
}
