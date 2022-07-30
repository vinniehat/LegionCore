using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LegionCore.Core.Models;
using LegionCore.Web.Models;
using Newtonsoft.Json;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();
    private DiscordSocketClient _client;
    private AppSettings _appSettings;

    private async Task MainAsync()
    {
        _appSettings =
            JsonConvert.DeserializeObject<AppSettings>(
                File.ReadAllText(@"LegionCore.Infrastructure/Data/appsettings.json"));
        _client = new DiscordSocketClient();
        _client.MessageReceived += CommandHandler;
        _client.Log += Log;


        await _client.LoginAsync(TokenType.Bot, _appSettings.Discord.Bot.Token);
        await _client.StartAsync();

        _client.Ready += () =>
        {
            Console.WriteLine("Bot is connected!");
            return Task.CompletedTask;
        };
        await Task.Delay(-1);
    }

    private static Task Log(LogMessage message)
    {
        if (message.Exception is CommandException cmdException)
        {
            Console.WriteLine($"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
                              + $" failed to execute in {cmdException.Context.Channel}.");
            Console.WriteLine(cmdException);
        }
        else
            Console.WriteLine($"[General/{message.Severity}] {message}");

        return Task.CompletedTask;
    }

    private Task CommandHandler(SocketMessage message)
    {

        if (!message.Content.StartsWith(_appSettings.Discord.Bot.Prefix))
            return Task.CompletedTask;
        if (message.Author.IsBot)
            return Task.CompletedTask;
        
        var command = message.Content.Substring(1, message.Content.Length - 1);
        
        

        return Task.CompletedTask;
    }
}