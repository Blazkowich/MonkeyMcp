using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using MonkeyMcp.Core.Exceptions;
using MonkeyMcp.Core.Models;
using MonkeyMcp.Core.Services;

namespace MonkeyMcp.Mcp.Tools;

[McpServerToolType]
public sealed class MonkeyTools(IMonkeyService monkeyService, ILogger<MonkeyTools> logger)
{
    private readonly IMonkeyService _monkeyService = monkeyService ?? throw new ArgumentNullException(nameof(monkeyService));
    private readonly ILogger<MonkeyTools> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [McpServerTool]
    [Description("Get a complete list of all available monkeys with their details.")]
    public async Task<string> GetMonkeys()
    {
        try
        {
            _logger.LogInformation("Retrieving all monkeys");
            var monkeys = await _monkeyService.GetMonkeysAsync();
            
            var result = JsonSerializer.Serialize(monkeys, MonkeyContext.Default.IReadOnlyListMonkey);
            _logger.LogInformation("Successfully retrieved {Count} monkeys", monkeys.Count);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve monkeys");
            throw;
        }
    }

    [McpServerTool]
    [Description("Get detailed information about a specific monkey by its name.")]
    public async Task<string> GetMonkey(
        [Description("The name of the monkey to retrieve (case-insensitive)")] string name)
    {
        try
        {
            _logger.LogInformation("Retrieving monkey: {MonkeyName}", name);
            var monkey = await _monkeyService.GetMonkeyAsync(name);
            
            var result = JsonSerializer.Serialize(monkey, MonkeyContext.Default.Monkey);
            _logger.LogInformation("Successfully retrieved monkey: {MonkeyName}", name);
            
            return result;
        }
        catch (MonkeyNotFoundException ex)
        {
            _logger.LogWarning("Monkey not found: {MonkeyName}", ex.MonkeyName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve monkey: {MonkeyName}", name);
            throw;
        }
    }

    [McpServerTool]
    [Description("Get monkey business - returns fun monkey emojis for entertainment.")]
    public static Task<string> GetMonkeyBusiness()
    {
        return Task.FromResult("🐵🐵🐵");
    }

    [McpServerTool]
    [Description("Refresh the monkey data cache from the remote API.")]
    public async Task<string> RefreshMonkeyCache()
    {
        try
        {
            _logger.LogInformation("Refreshing monkey cache");
            await _monkeyService.RefreshCacheAsync();
            _logger.LogInformation("Successfully refreshed monkey cache");
            
            return "Monkey cache refreshed successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to refresh monkey cache");
            throw;
        }
    }
}