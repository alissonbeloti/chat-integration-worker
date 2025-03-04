using Core.Domain.Enum;
using Core.Application.Factories;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class WorkerHealthCheck : IHealthCheck
{
    private readonly ChatFactory _chatFactory;

    public WorkerHealthCheck(ChatFactory chatFactory)
    {
        _chatFactory = chatFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var whatsAppChat = _chatFactory.CreateChat(MessagePlatform.WhatsApp);
            var instagramChat = _chatFactory.CreateChat(MessagePlatform.Instagram);

            var whatsAppConnected = await whatsAppChat.Connect();
            var instagramConnected = await instagramChat.Connect();

            if (whatsAppConnected && instagramConnected)
            {
                return HealthCheckResult.Healthy("Todos os serviços estão conectados");
            }

            return HealthCheckResult.Degraded("Alguns serviços não estão conectados");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Erro ao verificar conexões", ex);
        }
    }
} 