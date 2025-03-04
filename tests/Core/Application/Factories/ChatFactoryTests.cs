using Xunit;
using Moq;
using Infrastructure.Integration.Interfaces;
using Infrastructure.Factories;
using Microsoft.Extensions.Logging;
using Core.Domain.Enum;
using Infrastructure.Chat;
public class ChatFactoryTests
{
    private readonly ChatFactory _factory;

    public ChatFactoryTests()
    {
        _factory = new ChatFactory(Mock.Of<IServiceProvider>(), Mock.Of<ILogger<ChatFactory>>());
    }

    [Fact]
    public void CreateChat_ShouldReturnWhatsAppChat_WhenPlatformIsWhatsApp()
    {
        // Act
        var chat = _factory.CreateChat(MessagePlatform.WhatsApp);

        // Assert
        Assert.IsType<WhatsAppChat>(chat);
    }

    [Fact]
    public void CreateChat_ShouldReturnInstagramChat_WhenPlatformIsInstagram()
    {
        // Act
        var chat = _factory.CreateChat(MessagePlatform.Instagram);

        // Assert
        Assert.IsType<InstagramChat>(chat);
    }

    [Fact]
    public void CreateChat_ShouldThrowArgumentException_WhenPlatformIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _factory.CreateChat((MessagePlatform)999));
    }
} 