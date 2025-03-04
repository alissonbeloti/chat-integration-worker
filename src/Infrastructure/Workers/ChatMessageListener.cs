using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Infrastructure.Factories;
using Polly;
using Microsoft.Extensions.Hosting;
using Polly.Retry;
using Core.Domain.Enum;
using Core.Domain.Interfaces;

namespace Infrastructure.Workers
{
    public class ChatMessageListener : BackgroundService
    {
        private readonly ILogger<ChatMessageListener> _logger;
        private readonly ChatFactory _chatFactory;
        private readonly IMessageHandler _messageHandler;
        private readonly IConfiguration _configuration;
        private readonly AsyncRetryPolicy _reconnectPolicy;

        public ChatMessageListener(
            ILogger<ChatMessageListener> logger,
            ChatFactory chatFactory,
            IMessageHandler messageHandler,
            IConfiguration configuration)
        {
            _logger = logger;
            _chatFactory = chatFactory;
            _messageHandler = messageHandler;
            _configuration = configuration;
            
            // Política específica para reconexão com backoff exponencial mais longo
            _reconnectPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    5, // Mais tentativas para reconexão
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2, 4, 8, 16, 32 segundos
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            exception,
                            "Tentativa {RetryCount} de reconexão falhou. Aguardando {TimeSpan:g} antes da próxima tentativa",
                            retryCount,
                            timeSpan);
                    });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _reconnectPolicy.ExecuteAsync(async () =>
                    {
                        var whatsAppChat = _chatFactory.CreateChat(MessagePlatform.WhatsApp);
                        var instagramChat = _chatFactory.CreateChat(MessagePlatform.Instagram);

                        await whatsAppChat.Connect();
                        await instagramChat.Connect();

                        await Task.WhenAll(
                            ListenWhatsAppMessages(whatsAppChat, stoppingToken),
                            ListenInstagramMessages(instagramChat, stoppingToken)
                        );
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro fatal no serviço de mensagens. Tentando reiniciar em 1 minuto");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }

        private async Task ListenWhatsAppMessages(IChat chat, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Implementação específica para escutar mensagens do WhatsApp
                    // Isso dependerá da API/SDK que você está usando
                    await _messageHandler.HandleNewMessage(new Core.Domain.Entities.Message()); // nova mensagem refactoring
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagens do WhatsApp");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private async Task ListenInstagramMessages(IChat chat, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Implementação específica para escutar mensagens do Instagram
                    // Isso dependerá da API/SDK que você está usando
                    await _messageHandler.HandleNewMessage(new Core.Domain.Entities.Message());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagens do Instagram");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
} 