using Infrastructure.Chat;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Integration.Interfaces;
using Core.Domain.Enum;
using Core.Domain.Interfaces;

namespace Infrastructure.Factories
{
    public class ChatFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ChatFactory> _logger;

        public ChatFactory(
            IServiceProvider serviceProvider,
            ILogger<ChatFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IChat CreateChat(MessagePlatform platform)
        {
            _logger.LogInformation("Criando instância de chat para plataforma: {Platform}", platform);

            try
            {
                return platform switch
                {
                    MessagePlatform.WhatsApp => CreateWhatsAppChat(),
                    MessagePlatform.Instagram => CreateInstagramChat(),
                    _ => throw new ArgumentException($"Plataforma não suportada: {platform}", nameof(platform))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar instância de chat para plataforma: {Platform}", platform);
                throw;
            }
        }

        private IChat CreateWhatsAppChat()
        {
            var client = _serviceProvider.GetRequiredService<IWhatsAppClient>();
            var logger = _serviceProvider.GetRequiredService<ILogger<WhatsAppChat>>();
            var policies = _serviceProvider.GetRequiredService<RetryPolicies>();

            return new WhatsAppChat(client, logger, policies);
        }

        private IChat CreateInstagramChat()
        {
            var client = _serviceProvider.GetRequiredService<IInstagramClient>();
            var logger = _serviceProvider.GetRequiredService<ILogger<InstagramChat>>();
            var policies = _serviceProvider.GetRequiredService<RetryPolicies>();

            return new InstagramChat(client, logger, policies);
        }
    }
} 