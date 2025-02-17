namespace ChatBot.Features.Chat;

public class ChatRequestEndpoint(ChatService chatService) : FastEndpoints.Endpoint<ChatRequest, string>
{
    public override void Configure()
    {
        AllowAnonymous();
        Post("chatrequest");
    }

    public override async Task HandleAsync(ChatRequest chatRequest, CancellationToken cancellationToken)
    {
        var response = await chatService.QueryWithHistoryAsync(chatRequest.Query, cancellationToken);

        await SendStringAsync(response, cancellation: cancellationToken);
    }
}