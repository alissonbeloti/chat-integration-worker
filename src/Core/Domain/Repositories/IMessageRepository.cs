using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Message>> GetAllAsync(CancellationToken cancellationToken);
    Task<Message> AddAsync(Message message, CancellationToken cancellationToken);
    Task UpdateAsync(Message message, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task SaveMessageAsync(Message message);
} 