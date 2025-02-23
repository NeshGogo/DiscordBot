using DiscordBot;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using DiscordBot.Options;


var builder = Host.CreateApplicationBuilder();

builder.Configuration.AddConfiguration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());

OLLamaOptions oLLamaOptions = new();
builder.Configuration.GetSection(OLLamaOptions.SectionName).Bind(oLLamaOptions);

DiscordOptions discordOptions = new();
builder.Configuration.GetSection(DiscordOptions.SectionName).Bind(discordOptions);

builder.Services.AddChatClient(new OllamaChatClient(new Uri(oLLamaOptions.Url), oLLamaOptions.Model));

var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();

var bot = new Bot(chatClient, discordOptions);
await bot.StartAsync();