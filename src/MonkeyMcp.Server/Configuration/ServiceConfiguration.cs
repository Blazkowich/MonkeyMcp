using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonkeyMcp.Core.Configuration;
using MonkeyMcp.Core.Services;

namespace MonkeyMcp.Server.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddMonkeyServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<MonkeyServiceOptions>(
            configuration.GetSection(MonkeyServiceOptions.SectionName));
            
        services.AddHttpClient<IMonkeyService, MonkeyService>();
        services.AddSingleton<IMonkeyService, MonkeyService>();
        
        return services;
    }
}