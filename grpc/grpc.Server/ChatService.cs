using Grpc.Core;
using Grpc.Example;
using Microsoft.Extensions.Logging;

namespace grpc.Server;

public class ChatService : Grpc.Example.ChatService.ChatServiceBase
{
    private readonly ILogger<ChatService> _logger;

    public ChatService(ILogger<ChatService> logger)
    {
        _logger = logger;
    }

    public override async Task Chat(
        IAsyncStreamReader<ChatMessage> requestStream,
        IServerStreamWriter<ChatMessage> responseStream,
        ServerCallContext context)
    {
        _logger.LogInformation("Client connected: {Peer}", context.Peer);

        await foreach (var message in requestStream.ReadAllAsync(context.CancellationToken))
        {
            _logger.LogInformation("[{User}]: {Content}", message.User, message.Content);

            // Upon receiving a message, send a reply back to the client
            var reply = new ChatMessage
            {
                User = "Server",
                Content = $"Echo from server: [{message.User}] said \"{message.Content}\"",
                Timestamp = DateTime.UtcNow.ToString("O")
            };

            await responseStream.WriteAsync(reply);
        }

        _logger.LogInformation("Client disconnected: {Peer}", context.Peer);
    }
}
