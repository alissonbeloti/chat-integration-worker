public interface IMessageHandler
{
    Task HandleNewMessage(Message message);
} 