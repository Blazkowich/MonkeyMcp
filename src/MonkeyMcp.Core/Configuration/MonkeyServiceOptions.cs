namespace MonkeyMcp.Core.Configuration;

public sealed class MonkeyServiceOptions
{
    public const string SectionName = "MonkeyService";
    
    public string ApiUrl { get; set; } = "https://www.montemagno.com/monkeys.json";
    public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromHours(1);
    public int HttpTimeoutSeconds { get; set; } = 30;
}