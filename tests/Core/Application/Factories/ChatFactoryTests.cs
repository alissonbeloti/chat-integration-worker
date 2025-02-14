using Xunit;
using Moq;
using Core.Infrastructure.Chat;
using Core.Domain.Interfaces;

public class ChatFactoryTests
{
    private readonly Mock<IWhatsAppClient> _whatsAppClientMock;
    private readonly Mock<IInstagramClient> _instagramClientMock;
    private readonly ChatFactory _factory;

    public ChatFactoryTests()
    {
        _whatsAppClientMock = new Mock<IWhatsAppClient>();
        _instagramClientMock = new Mock<IInstagramClient>();
        _factory = new ChatFactory(_whatsAppClientMock.Object, _instagramClientMock.Object);
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