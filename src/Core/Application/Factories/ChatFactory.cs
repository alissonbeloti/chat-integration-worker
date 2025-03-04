using Core.Domain.Enum;
using Core.Domain.Interfaces;

namespace Core.Application.Factories;
public class ChatFactory
{
    private readonly IChat _whatsAppClient;
    private readonly IChat _instagramClient;

    public ChatFactory(IChat whatsAppClient, IChat instagramClient)
    {
        _whatsAppClient = whatsAppClient;
        _instagramClient = instagramClient;
    }

    public IChat CreateChat(MessagePlatform platform)
    {
        return platform switch
        {
            MessagePlatform.WhatsApp => _whatsAppClient,
            MessagePlatform.Instagram => _instagramClient,
            _ => throw new ArgumentException("Plataforma não suportada", nameof(platform))
        };
    }
}
