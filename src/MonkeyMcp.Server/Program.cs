using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonkeyMcp.Mcp.Prompts;
using MonkeyMcp.Mcp.Resources;
using MonkeyMcp.Mcp.Tools;
using MonkeyMcp.Server.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddMonkeyServices();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithPrompts<MonkeyPrompts>()
    .WithResources<MonkeyResources>()
    .WithTools<MonkeyTools>();

var host = builder.Build();
await host.RunAsync();