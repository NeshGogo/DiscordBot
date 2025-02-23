using Discord;
using Discord.WebSocket;
using DiscordBot.Options;
using Microsoft.Extensions.AI;

namespace DiscordBot;

internal class Bot
{
    private readonly DiscordSocketClient _client;
    private readonly string _token;
    private readonly IChatClient _aiChatClient;

    public Bot(IChatClient aiChatClient, DiscordOptions options)
    {
        _aiChatClient = aiChatClient;
        _token = options.Token;
        _client = new DiscordSocketClient();
        _client.MessageReceived += MessageReceivedAsync;
    }

    public async Task StartAsync()
    {
        if (string.IsNullOrEmpty(_token))
        {
            Console.WriteLine("You must register your token in an enviroment variable as TOKEN");
            return;
        }

        Console.WriteLine("Starting bot...");
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();
        Console.WriteLine("Bot started.");
        await Task.Delay(-1);
    }

    private async Task MessageReceivedAsync(SocketMessage message)
    {
        if (message.Author.IsBot) return;

        var response = await _aiChatClient.GetResponseAsync(message.Content);

        await ReplayAsync(message, response.Message.Text);
    }

    private async Task ReplayAsync(SocketMessage message, string response) =>
        await message.Channel.SendMessageAsync(response);

}
