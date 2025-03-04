using Microsoft.Extensions.Logging;
using Infrastructure.Integration.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Enum;
using Core.Domain.Interfaces;

namespace Infrastructure.Chat;
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
        var retryPolicyForMessages = _policies.CreateDefaultRetryPolicy<IEnumerable<WhatsAppMessage>>();

        var messages = await _policies.CreateCircuitBreakerPolicy<IEnumerable<WhatsAppMessage>>()
            .ExecuteAsync(async () =>
                await retryPolicyForMessages.ExecuteAsync(async () =>
                    await _client.GetMessages(conversationId)));

        return messages.Select(m => new Message
        {
            Id = Guid.Parse(m.Id),
            Content = m.Content,
            From = m.From,
            To = m.To,
            CreatedAt = m.Timestamp,
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