using Xunit;
using Moq;
using Core.Application.Factories;
using Core.Domain.Enum;
using Core.Domain.Interfaces;


public class SendMessageUseCaseTests
{
    private readonly Mock<ChatFactory> _chatFactoryMock;
    private readonly Mock<IChat> _chatMock;
    private readonly SendMessageUseCase _useCase;

    public SendMessageUseCaseTests()
    {
        _chatMock = new Mock<IChat>();
        _chatFactoryMock = new Mock<ChatFactory>(MockBehavior.Strict);
        _useCase = new SendMessageUseCase(_chatFactoryMock.Object);
    }

    [Theory]
    [InlineData(MessagePlatform.WhatsApp)]
    [InlineData(MessagePlatform.Instagram)]
    public async Task Execute_ShouldSendMessageAndReturnTrue_WhenSuccessful(MessagePlatform platform)
    {
        // Arrange
        const string recipient = "test_recipient";
        const string message = "test_message";

        _chatMock.Setup(x => x.Connect()).ReturnsAsync(true);
        _chatMock.Setup(x => x.SendMessage(recipient, message)).ReturnsAsync(true);
        _chatMock.Setup(x => x.Disconnect()).ReturnsAsync(true);

        _chatFactoryMock
            .Setup(x => x.CreateChat(platform))
            .Returns(_chatMock.Object);

        // Act
        var result = await _useCase.Execute(platform, recipient, message);

        // Assert
        Assert.True(result);
        _chatMock.Verify(x => x.Connect(), Times.Once);
        _chatMock.Verify(x => x.SendMessage(recipient, message), Times.Once);
        _chatMock.Verify(x => x.Disconnect(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldDisconnectEvenWhenSendMessageFails()
    {
        // Arrange
        const string recipient = "test_recipient";
        const string message = "test_message";

        _chatMock.Setup(x => x.Connect()).ReturnsAsync(true);
        _chatMock.Setup(x => x.SendMessage(recipient, message)).ThrowsAsync(new Exception("Send failed"));
        _chatMock.Setup(x => x.Disconnect()).ReturnsAsync(true);

        _chatFactoryMock
            .Setup(x => x.CreateChat(MessagePlatform.WhatsApp))
            .Returns(_chatMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => 
            _useCase.Execute(MessagePlatform.WhatsApp, recipient, message));

        _chatMock.Verify(x => x.Disconnect(), Times.Once);
    }
} 