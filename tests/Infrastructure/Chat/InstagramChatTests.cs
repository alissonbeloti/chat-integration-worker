using Xunit;
using Moq;
using Core.Domain.Entities;
using Core.Infrastructure.Chat;

public class InstagramChatTests
{
    private readonly Mock<IInstagramClient> _clientMock;
    private readonly InstagramChat _chat;

    public InstagramChatTests()
    {
        _clientMock = new Mock<IInstagramClient>();
        _chat = new InstagramChat(_clientMock.Object);
    }

    [Fact]
    public async Task Connect_ShouldCallLoginAndReturnTrue_WhenSuccessful()
    {
        // Arrange
        _clientMock.Setup(x => x.Login()).ReturnsAsync(true);

        // Act
        var result = await _chat.Connect();

        // Assert
        Assert.True(result);
        _clientMock.Verify(x => x.Login(), Times.Once);
    }

    [Fact]
    public async Task Disconnect_ShouldCallLogoutAndReturnTrue_WhenSuccessful()
    {
        // Arrange
        _clientMock.Setup(x => x.Logout()).ReturnsAsync(true);

        // Act
        var result = await _chat.Disconnect();

        // Assert
        Assert.True(result);
        _clientMock.Verify(x => x.Logout(), Times.Once);
    }

    [Fact]
    public async Task SendMessage_ShouldCallSendDirectMessageAndReturnTrue_WhenSuccessful()
    {
        // Arrange
        const string recipient = "test_recipient";
        const string message = "test_message";
        _clientMock.Setup(x => x.SendDirectMessage(recipient, message)).ReturnsAsync(true);

        // Act
        var result = await _chat.SendMessage(recipient, message);

        // Assert
        Assert.True(result);
        _clientMock.Verify(x => x.SendDirectMessage(recipient, message), Times.Once);
    }

    [Fact]
    public async Task GetMessages_ShouldReturnMappedMessages_WhenSuccessful()
    {
        // Arrange
        const string conversationId = "test_conversation";
        var instagramMessages = new[]
        {
            new InstagramDirectMessage 
            { 
                Id = "1",
                Text = "Hello",
                SenderId = "sender1",
                ReceiverId = "receiver1",
                CreatedAt = DateTime.UtcNow
            }
        };

        _clientMock.Setup(x => x.GetDirectMessages(conversationId))
            .ReturnsAsync(instagramMessages);

        // Act
        var messages = await _chat.GetMessages(conversationId);

        // Assert
        var messageList = messages.ToList();
        Assert.Single(messageList);
        Assert.Equal(instagramMessages[0].Id, messageList[0].Id);
        Assert.Equal(instagramMessages[0].Text, messageList[0].Content);
        Assert.Equal(MessagePlatform.Instagram, messageList[0].Platform);
    }
} 