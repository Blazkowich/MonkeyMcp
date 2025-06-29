using MonkeyMcp.Core.Models;

namespace MonkeyMcp.Core.Services;

public interface IMonkeyService
{
    Task<IReadOnlyList<Monkey>> GetMonkeysAsync(CancellationToken cancellationToken = default);
    Task<Monkey> GetMonkeyAsync(string name, CancellationToken cancellationToken = default);
    Task RefreshCacheAsync(CancellationToken cancellationToken = default);
}