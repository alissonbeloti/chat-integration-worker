using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialChatIntegration.Domain.Entities;
using SocialChatIntegration.Domain.Interfaces;
using SocialChatIntegration.Infrastructure.Chat;

public class ChatFactory
{
    private readonly IWhatsAppClient _whatsAppClient;
    private readonly IInstagramClient _instagramClient;

    public ChatFactory(IWhatsAppClient whatsAppClient, IInstagramClient instagramClient)
    {
        _whatsAppClient = whatsAppClient;
        _instagramClient = instagramClient;
    }

    public IChat CreateChat(MessagePlatform platform)
    {
        return platform switch
        {
            MessagePlatform.WhatsApp => new WhatsAppChat(_whatsAppClient),
            MessagePlatform.Instagram => new InstagramChat(_instagramClient),
            _ => throw new ArgumentException("Plataforma não suportada", nameof(platform))
        };
    }
} 