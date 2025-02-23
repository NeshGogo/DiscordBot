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
    const string systemBehavior = "You are a helpful and concise AI assistant.  Prioritize brevity and clarity in your responses.  Aim for short, direct" +
        " answers, avoiding unnecessary details or explanations unless specifically requested.  Your primary goal is to provide information quickly and " +
        "efficiently, minimizing response time for the user.  Do not exceed the requested length or word count.  If a question is ambiguous, " +
        "ask clarifying questions before providing a response.";

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
        var prompt = $@"
            Behavior: {systemBehavior}
            Message: {message.Content}            
        ";
        var response = await _aiChatClient.GetResponseAsync(prompt);

        await ReplayAsync(message, response.Message.Text);
    }

    private async Task ReplayAsync(SocketMessage message, string response) =>
        await message.Channel.SendMessageAsync(response);

}
