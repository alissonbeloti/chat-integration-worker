using Infrastructure.Workers;
using Infrastructure.Integration.Interfaces;
using Infrastructure.Factories;
using ChatFactory = Core.Application.Factories.ChatFactory;
using Core.Application.Handlers;
using Infrastructure.Integration;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Configura as políticas de resiliência
                services.Configure<ResilienceSettings>(
                    hostContext.Configuration.GetSection("ResilienceSettings"));
                
                services.AddSingleton<RetryPolicies>();
                services.AddHostedService<ChatMessageListener>();
                
                // Registra as dependências
                services.AddSingleton<ChatFactory>();
                services.AddSingleton<IWhatsAppClient, WhatsAppClient>();
                services.AddSingleton<IInstagramClient, InstagramClent>();
                services.AddScoped<IMessageHandler, MessageHandler>();
                
                // Adiciona logging
                services.AddLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    // Adicione outros providers de logging conforme necessário
                });
            })
            .Build();

        host.Run();
    }
}