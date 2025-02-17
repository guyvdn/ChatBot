using System.ComponentModel.DataAnnotations;

namespace ChatBot.Features.Chat;

public class ChatCompletionsOptions
{
    [Required] 
    public string Endpoint { get; set; } = null!;

    [Required] 
    public string Key { get; set; } = null!;
}