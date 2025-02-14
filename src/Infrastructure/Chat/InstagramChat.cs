using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialChatIntegration.Core.Domain.Entities;
using SocialChatIntegration.Core.Domain.Interfaces;
using SocialChatIntegration.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Core.Domain.Settings;

public class InstagramChat : IChat
{
    private readonly IInstagramClient _client;
    private readonly ILogger<InstagramChat> _logger;
    private readonly RetryPolicies _policies;

    public InstagramChat(
        IInstagramClient client,
        ILogger<InstagramChat> logger,
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
                .ExecuteAsync(async () => await _client.Login()));
    }

    public async Task<bool> Disconnect()
    {
        return await _policies.CreateDefaultRetryPolicy<bool>()
            .ExecuteAsync(async () => await _client.Logout());
    }

    public async Task<IEnumerable<Message>> GetMessages(string conversationId)
    {
        var retryPolicyForMessages = _policies.CreateDefaultRetryPolicy<IEnumerable<InstagramDirectMessage>>(_logger);
        
        var messages = await _policies.CreateCircuitBreakerPolicy<IEnumerable<InstagramDirectMessage>>()
            .ExecuteAsync(async () => await retryPolicyForMessages.ExecuteAsync(async () =>
                await _client.GetDirectMessages(conversationId)));

        return messages.Select(m => new Message
        {
            Id = m.Id,
            Content = m.Text,
            SenderId = m.SenderId,
            ReceiverId = m.ReceiverId,
            Timestamp = m.CreatedAt,
            Platform = MessagePlatform.Instagram
        });
    }

    public async Task<bool> SendMessage(string recipient, string message)
    {
        return await _policies.CreateCircuitBreakerPolicy<bool>()
            .ExecuteAsync(async () => await _policies.CreateDefaultRetryPolicy<bool>()
                .ExecuteAsync(async () => await _client.SendDirectMessage(recipient, message)));
    }
} 