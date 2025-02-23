namespace DiscordBot.Options;

internal sealed class OLLamaOptions
{
    public const string SectionName = "OLlama";
    public string Model { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
