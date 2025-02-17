using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using ChatRole = Microsoft.Extensions.AI.ChatRole;

namespace ChatBot.Features.Chat;

public sealed class ChatService : IDisposable
{
    private readonly IChatClient _client;
    private readonly ChatOptions _chatOptions = new() { Temperature = 1, MaxOutputTokens = 500, TopP = 1 };
    private readonly List<ChatMessage> _conversationHistory = [];

    public ChatService(IOptions<ChatCompletionsOptions> options)
    {
        var optionsValue = options.Value;

        var endpoint = new Uri(optionsValue.Endpoint);
        var credential = new AzureKeyCredential(optionsValue.Key);

        _client = new ChatCompletionsClient(endpoint, credential).AsChatClient("gpt-4o");
        //_client = new ChatCompletionsClient(endpoint, credential).AsChatClient("gpt-4o-mini");
        //_client = new ChatCompletionsClient(endpoint, credential).AsChatClient("DeepSeek-R1");

        _conversationHistory.Add(new ChatMessage(ChatRole.System, "Act as a 5 year old"));
    }

    public async Task<string> QueryWithHistoryAsync(string query, CancellationToken cancellationToken = default)
    {
        _conversationHistory.Add(new ChatMessage(ChatRole.User, query));

        var chatCompletion = await _client.CompleteAsync(_conversationHistory, _chatOptions, cancellationToken);

        _conversationHistory.Add(chatCompletion.Message);

        return chatCompletion.Message.Text!;
    }

    public async Task<string> QueryAsync(string role, string query, CancellationToken cancellationToken = default)
    {
        ChatMessage[] messages = [new(ChatRole.System, role), new(ChatRole.User, query)];

        var chatCompletion = await _client.CompleteAsync(messages, _chatOptions, cancellationToken);

        return chatCompletion.Message.Text!;
    }

    public async Task<T> QueryAsync<T>(string role, string query, CancellationToken cancellationToken = default)
    {
        ChatMessage[] messages = [new(ChatRole.System, role), new(ChatRole.User, query)];

        var chatCompletion = await _client.CompleteAsync<T>(messages, _chatOptions, useNativeJsonSchema: true, cancellationToken);

        return chatCompletion.Result;
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}