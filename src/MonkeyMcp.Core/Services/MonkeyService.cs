using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyMcp.Core.Configuration;
using MonkeyMcp.Core.Exceptions;
using MonkeyMcp.Core.Models;

namespace MonkeyMcp.Core.Services;

public sealed class MonkeyService : IMonkeyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MonkeyService> _logger;
    private readonly MonkeyServiceOptions _options;
    private readonly SemaphoreSlim _cacheLock = new(1, 1);
    
    private List<Monkey>? _cachedMonkeys;
    private DateTime _lastCacheUpdate = DateTime.MinValue;

    public MonkeyService(
        HttpClient httpClient,
        ILogger<MonkeyService> logger,
        IOptions<MonkeyServiceOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<IReadOnlyList<Monkey>> GetMonkeysAsync(CancellationToken cancellationToken = default)
    {
        await EnsureCacheIsCurrentAsync(cancellationToken);
        return _cachedMonkeys != null ? _cachedMonkeys.AsReadOnly() : Array.Empty<Monkey>();
    }

    public async Task<Monkey> GetMonkeyAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Monkey name cannot be null or empty.", nameof(name));

        var monkeys = await GetMonkeysAsync(cancellationToken);
        var monkey = monkeys.FirstOrDefault(m => 
            string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));

        return monkey ?? throw new MonkeyNotFoundException(name);
    }

    public async Task RefreshCacheAsync(CancellationToken cancellationToken = default)
    {
        await _cacheLock.WaitAsync(cancellationToken);
        try
        {
            await LoadMonkeysFromApiAsync(cancellationToken);
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    private async Task EnsureCacheIsCurrentAsync(CancellationToken cancellationToken)
    {
        if (IsCacheExpired())
        {
            await RefreshCacheAsync(cancellationToken);
        }
    }

    private bool IsCacheExpired()
    {
        return _cachedMonkeys == null || 
               DateTime.UtcNow - _lastCacheUpdate > _options.CacheExpiration;
    }

    private async Task LoadMonkeysFromApiAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Loading monkeys from API: {ApiUrl}", _options.ApiUrl);
            
            var response = await _httpClient.GetAsync(_options.ApiUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            var monkeys = await response.Content.ReadFromJsonAsync(
                MonkeyContext.Default.ListMonkey, cancellationToken);

            _cachedMonkeys = monkeys ?? [];
            _lastCacheUpdate = DateTime.UtcNow;
            
            _logger.LogInformation("Successfully loaded {Count} monkeys", _cachedMonkeys.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load monkeys from API");
            _cachedMonkeys ??= [];
        }
    }
}