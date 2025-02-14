using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialChatIntegration.Core.Domain.Entities;
using SocialChatIntegration.Core.Domain.Interfaces;
using SocialChatIntegration.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using Core.Domain.Settings;

public class WhatsAppChat : IChat
{
    private readonly IWhatsAppClient _client;
    private readonly ILogger<WhatsAppChat> _logger;
    private readonly RetryPolicies _policies;

    public WhatsAppChat(
        IWhatsAppClient client,
        ILogger<WhatsAppChat> logger,
        RetryPolicies policies)
    {
        _client = client;
        _logger = logger;
        _policies = policies;
    }

    public async Task<bool> Connect()
    {
        return await _policies.CreateCircuitBreakerPolicy<bool>()
            .ExecuteAsync(async () => await _policies.CreateDefaultRetryPolicy<bool>()
                .ExecuteAsync(async () => await _client.Initialize()));
    }

    public async Task<bool> Disconnect()
    {
        return await _policies.CreateDefaultRetryPolicy<bool>()
            .ExecuteAsync(async () => await _client.Disconnect());
    }

    public async Task<IEnumerable<Message>> GetMessages(string conversationId)
    {
        var retryPolicyForMessages = _policies.CreateDefaultRetryPolicy<IEnumerable<WhatsAppMessage>>(_logger);
        
        var messages = await _policies.CreateCircuitBreakerPolicy<bool>()
            .ExecuteAsync(async () =>
                await retryPolicyForMessages.ExecuteAsync(async () =>
                    await _client.GetMessages(conversationId)));

        return messages.Select(m => new Message
        {
            Id = m.Id,
            Content = m.Content,
            SenderId = m.From,
            ReceiverId = m.To,
            Timestamp = m.Timestamp,
            Platform = MessagePlatform.WhatsApp
        });
    }

    public async Task<bool> SendMessage(string recipient, string message)
    {
        return await _policies.CreateCircuitBreakerPolicy<bool>()
            .ExecuteAsync(async () => await _policies.CreateDefaultRetryPolicy<bool>()
                .ExecuteAsync(async () => await _client.SendMessage(recipient, message)));
    }
} 