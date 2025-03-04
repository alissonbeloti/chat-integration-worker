
using Core.Domain.Entities;

namespace Infrastructure.Factories;

public interface IInstagramClient
{
    Task<IEnumerable<InstagramDirectMessage>> GetDirectMessages(string conversationId);
    Task<bool> Login();
    Task<bool> Logout();
    Task<bool> SendDirectMessage(string recipient, string message);
}