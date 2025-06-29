using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyMcp.Core.Models;
using MonkeyMcp.Core.Services;

namespace MonkeyMcp.Server.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddMonkeyServices(
        this IServiceCollection services)
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables();

        var builtConfig = configBuilder.Build();

        services.AddOptions<MonkeyServiceOptions>()
            .Configure(options => builtConfig.GetSection(MonkeyServiceOptions.SectionName).Bind(options))
            .Validate(options => !string.IsNullOrEmpty(options.ApiUrl), "ApiUrl must be configured")
            .Validate(options => options.HttpTimeoutSeconds > 0, "HttpTimeoutSeconds must be greater than 0")
            .ValidateOnStart();
            
        services.AddHttpClient<IMonkeyService, MonkeyService>();
        
        return services;
    }
}