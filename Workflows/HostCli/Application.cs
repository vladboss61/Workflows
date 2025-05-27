using HostCli.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostCli;
public class Application
{
    private readonly ILogger<Application> _logger;
    private readonly MyAppSettings _settings;

    public Application(ILogger<Application> logger, IOptions<MyAppSettings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message from config: {Message}", _settings.Message ?? "none");

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"Message from config: {_settings.Message}");

            // your main loop logic here
            await Task.Delay(2500, cancellationToken); // Example
        }
    }
}
