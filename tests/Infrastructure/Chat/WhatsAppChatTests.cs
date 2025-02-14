using Xunit;
using Moq;
using Core.Domain.Entities;
using Core.Infrastructure.Chat;

public class WhatsAppChatTests
{
    private readonly Mock<IWhatsAppClient> _clientMock;
    private readonly WhatsAppChat _chat;

    public WhatsAppChatTests()
    {
        _clientMock = new Mock<IWhatsAppClient>();
        _chat = new WhatsAppChat(_clientMock.Object);
    }

    [Fact]
    public async Task Connect_ShouldCallInitializeAndReturnTrue_WhenSuccessful()
    {
        // Arrange
        _clientMock.Setup(x => x.Initialize()).ReturnsAsync(true);

        // Act
        var result = await _chat.Connect();

        // Assert
        Assert.True(result);
        _clientMock.Verify(x => x.Initialize(), Times.Once);
    }

    [Fact]
    public async Task Disconnect_ShouldCallDisconnectAndReturnTrue_WhenSuccessful()
    {
        // Arrange
        _clientMock.Setup(x => x.Disconnect()).ReturnsAsync(true);

        // Act
        var result = await _chat.Disconnect();

        // Assert
        Assert.True(result);
        _clientMock.Verify(x => x.Disconnect(), Times.Once);
    }

    [Fact]
    public async Task SendMessage_ShouldCallSendMessageAndReturnTrue_WhenSuccessful()
    {
        // Arrange
        const string recipient = "test_recipient";
        const string message = "test_message";
        _clientMock.Setup(x => x.SendMessage(recipient, message)).ReturnsAsync(true);

        // Act
        var result = await _chat.SendMessage(recipient, message);

        // Assert
        Assert.True(result);
        _clientMock.Verify(x => x.SendMessage(recipient, message), Times.Once);
    }

    [Fact]
    public async Task GetMessages_ShouldReturnMappedMessages_WhenSuccessful()
    {
        // Arrange
        const string conversationId = "test_conversation";
        var whatsAppMessages = new[]
        {
            new WhatsAppMessage 
            { 
                Id = "1",
                Content = "Hello",
                From = "sender1",
                To = "receiver1",
                Timestamp = DateTime.UtcNow
            }
        };

        _clientMock.Setup(x => x.GetMessages(conversationId))
            .ReturnsAsync(whatsAppMessages);

        // Act
        var messages = await _chat.GetMessages(conversationId);

        // Assert
        var messageList = messages.ToList();
        Assert.Single(messageList);
        Assert.Equal(whatsAppMessages[0].Id, messageList[0].Id);
        Assert.Equal(whatsAppMessages[0].Content, messageList[0].Content);
        Assert.Equal(MessagePlatform.WhatsApp, messageList[0].Platform);
    }
} 