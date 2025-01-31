using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace ChatBot.Features.Chat;

public sealed class ChatService: IDisposable
{
    private readonly IChatClient _client;
    private readonly ChatOptions _chatOptions = new() { Temperature = 1, MaxOutputTokens = 100 };
    private readonly List<ChatMessage> _conversationHistory = [];

    public ChatService()
    {
        var endpoint = new Uri("https://models.inference.ai.azure.com");

        var credential = new AzureKeyCredential(Environment.GetEnvironmentVariable("GH_TOKEN")!);

        _client = new ChatCompletionsClient(endpoint, credential).AsChatClient("gpt-4o");

        _conversationHistory.Add(new ChatMessage(ChatRole.System, "Act as a five year old"));
    }

    public async Task<string> QueryAsync(string query, CancellationToken cancellationToken)
    {
        _conversationHistory.Add(new ChatMessage(ChatRole.User, query));
        
        var chatCompletion = await _client.CompleteAsync(_conversationHistory, _chatOptions, cancellationToken);

        _conversationHistory.Add(chatCompletion.Message);

        return chatCompletion.Message.Text!;
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}