using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using MonkeyMcp.Core.Exceptions;
using MonkeyMcp.Core.Models;
using MonkeyMcp.Core.Services;

namespace MonkeyMcp.Mcp.Resources;

[McpServerResourceType]
public sealed class MonkeyResources(IMonkeyService monkeyService, ILogger<MonkeyResources> logger)
{
    private readonly IMonkeyService _monkeyService = monkeyService ?? throw new ArgumentNullException(nameof(monkeyService));
    private readonly ILogger<MonkeyResources> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [McpServerResource(
        UriTemplate = "monkeymcp://monkeys/baboon", 
        Name = "Baboon Details", 
        MimeType = "application/json")]
    [Description("Get comprehensive details about the baboon monkey species.")]
    public async Task<string> GetBaboonDetails()
    {
        try
        {
            _logger.LogInformation("Retrieving baboon details");
            var baboon = await _monkeyService.GetMonkeyAsync("Baboon");
            return JsonSerializer.Serialize(baboon, MonkeyContext.Default.Monkey);
        }
        catch (MonkeyNotFoundException ex)
        {
            _logger.LogWarning("Baboon not found: {Message}", ex.Message);
            throw;
        }
    }

    [McpServerResource(
        UriTemplate = "monkeymcp://monkeys/{name}", 
        Name = "Monkey Details",
        MimeType = "application/json")]
    [Description("Get detailed information about any monkey species by name.")]
    public async Task<string> GetMonkeyDetails(string name)
    {
        try
        {
            _logger.LogInformation("Retrieving details for monkey: {MonkeyName}", name);
            var monkey = await _monkeyService.GetMonkeyAsync(name);
            return JsonSerializer.Serialize(monkey, MonkeyContext.Default.Monkey);
        }
        catch (MonkeyNotFoundException ex)
        {
            _logger.LogWarning("Monkey not found: {MonkeyName}", ex.MonkeyName);
            throw;
        }
    }
}