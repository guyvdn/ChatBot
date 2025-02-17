using Microsoft.Extensions.Configuration;

namespace ChatBot.Tests;

public static class Configuration
{
    private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
        .AddUserSecrets(typeof(ChatBot.AssemblyMarker).Assembly)
        .Build();

    public static T GetOptions<T>(string key) where T : new()
    {
        var options = new T();
        Config.Bind(key, options);
        return options;
    }
}