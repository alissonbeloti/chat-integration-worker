using Core.Domain.Entities;
using Core.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ILogger<MessageRepository> _logger;
    private readonly List<Message> _messages;

    public MessageRepository(ILogger<MessageRepository> logger)
    {
        _logger = logger;
        _messages = new List<Message>();
    }

    public async Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting message with id: {Id}", id);
        return await Task.FromResult(_messages.FirstOrDefault(m => m.Id == id));
    }

    public async Task<IEnumerable<Message>> GetAllAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all messages");
        return await Task.FromResult(_messages.AsEnumerable());
    }

    public async Task<Message> AddAsync(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new message from {From} to {To}", message.From, message.To);
        
        message.Id = Guid.NewGuid();
        message.CreatedAt = DateTime.UtcNow;
        message.Status = MessageStatus.Pending;
        
        _messages.Add(message);
        
        return await Task.FromResult(message);
    }

    public async Task UpdateAsync(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating message with id: {Id}", message.Id);
        
        var existingMessage = _messages.FirstOrDefault(m => m.Id == message.Id);
        if (existingMessage == null)
        {
            throw new KeyNotFoundException($"Message with id {message.Id} not found");
        }

        var index = _messages.IndexOf(existingMessage);
        message.UpdatedAt = DateTime.UtcNow;
        _messages[index] = message;
        
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting message with id: {Id}", id);
        
        var message = _messages.FirstOrDefault(m => m.Id == id);
        if (message == null)
        {
            throw new KeyNotFoundException($"Message with id {id} not found");
        }

        _messages.Remove(message);
        await Task.CompletedTask;
    }

    public Task SaveMessageAsync(Message message)
    {
        throw new NotImplementedException();
    }
} 