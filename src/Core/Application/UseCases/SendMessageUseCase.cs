using System;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Application.Factories;

public class SendMessageUseCase
{
    private readonly ChatFactory _chatFactory;

    public SendMessageUseCase(ChatFactory chatFactory)
    {
        _chatFactory = chatFactory;
    }

    public async Task<bool> Execute(MessagePlatform platform, string recipient, string message)
    {
        var chat = _chatFactory.CreateChat(platform);
        await chat.Connect();
        
        try
        {
            return await chat.SendMessage(recipient, message);
        }
        finally
        {
            await chat.Disconnect();
        }
    }
} 