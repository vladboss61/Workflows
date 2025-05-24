using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace HostCli;

// --- App class ---
public class App
{
    private readonly ILogger<App> _logger;
    private readonly MySettings _settings;

    public App(ILogger<App> logger, IOptions<MySettings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public Task Run()
    {
        _logger.LogInformation("Message from config: {Message}", _settings.Message ?? "none");
        Console.WriteLine($"Message from config: {_settings.Message}");
        return Task.CompletedTask;
    }
}

public class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
        .CreateLogger();

        try
        {
            Log.Information("Starting up");

            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog(Log.Logger)
                .ConfigureAppConfiguration(builder => builder.AddConfiguration(configuration))
                .ConfigureServices((context, services) =>
                {
                    services.Configure<MySettings>(configuration.GetSection("MyApp"));
                    services.AddTransient<App>();
                })
                .Build();

            var app = host.Services.GetRequiredService<App>();
            await app.Run();

            Log.Information("Shutting down");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
