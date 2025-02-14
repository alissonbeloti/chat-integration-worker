using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Core.Application.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly ILogger<MessageHandler> _logger;
        private readonly IMessageRepository _messageRepository;
        // Outras dependências necessárias

        public MessageHandler(
            ILogger<MessageHandler> logger,
            IMessageRepository messageRepository)
        {
            _logger = logger;
            _messageRepository = messageRepository;
        }

        public async Task HandleNewMessage(Message message)
        {
            try
            {
                // Lógica de processamento da mensagem
                _logger.LogInformation($"Nova mensagem recebida de {message.SenderId} via {message.Platform}");

                // Salva a mensagem
                await _messageRepository.SaveMessageAsync(message);

                // Processa regras de negócio
                // Por exemplo: resposta automática, notificações, etc.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem");
                throw;
            }
        }
    }
} 