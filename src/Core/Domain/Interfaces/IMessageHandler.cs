using Core.Domain.Entities;

public interface IMessageHandler
{
    Task HandleNewMessage(Message message);
} 