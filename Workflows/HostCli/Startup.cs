using HostCli.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Threading.Tasks;

namespace HostCli;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    private readonly ILogger _logger;

    public Startup(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task ConfigureServices(IServiceCollection services)
    {
        services.Configure<MyApp>(_configuration.GetSection(nameof(MyApp)));

        services.AddTransient<App>();
        await Task.CompletedTask;
    }
}
