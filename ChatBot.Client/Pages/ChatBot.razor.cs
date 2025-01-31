using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace ChatBot.Client.Pages;

public partial class ChatBot
{
    [Inject]
    private HttpClient HttpClient { get; set; } = null!;

    private string? Input { get; set; }
    private string? Output { get; set; }

    private async Task AskQuestion()
    {
        var response = await HttpClient.PostAsJsonAsync("chatrequest", new { Query = Input });

        if (response.IsSuccessStatusCode)
        {
            Output = await response.Content.ReadAsStringAsync();
        }
        else
        {
            Output = response.ReasonPhrase;
        }
    }
}