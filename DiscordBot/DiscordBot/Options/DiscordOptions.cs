﻿namespace DiscordBot.Options;

internal sealed class DiscordOptions
{
    public const string SectionName = "Discord";
    public string Token { get; set; } = string.Empty;
}
