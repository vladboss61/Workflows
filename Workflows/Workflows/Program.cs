namespace WorkflowsEx;

using Refit;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowsEx.GithubApi;
using WorkflowsEx.Workflows;
using Microsoft.Extensions.DependencyInjection;
using WorkflowsEx.Infrastructure;
using Microsoft.Extensions.Logging;
using WorkflowsEx.Workflows.Data;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        ConfigurationSettings settings = JsonSerializer.Deserialize<ConfigurationSettings>(File.ReadAllText("appsettings.json"));

        ServiceCollection services = new ServiceCollection();

        services.AddOptions<ConfigurationSettings>().Configure(x => x.GithubUrl = settings.GithubUrl);
        
        services.AddTransient<LoggingDelegatingHandler>();

        AddRemoteRefitClient<IGithubRepository>(services, (settings) => settings.GithubUrl);

        services.AddLogging(builder =>
        {
            // Optional: filter out low-level logs
            // We can read in from appsettings.json or configure here.
            builder
                .AddFilter("Default", LogLevel.Information)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddConsole();
        });

        services.AddWorkflow(); // in-memory persistence

        services.AddSingleton<Application>();

        ConfigureWorkflows(services);

        await services.BuildServiceProvider().GetRequiredService<Application>().RunAsync();
    }

    private static void AddRemoteRefitClient<T>(ServiceCollection services, Func<ConfigurationSettings, string> urlConfigure)
        where T : class
    {
        var settings = services.BuildServiceProvider().GetRequiredService<ConfigurationSettings>();
        services.AddRefitClient<T>().ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(urlConfigure(settings));

            // Required by many HTTP servers.
            client.DefaultRequestHeaders.UserAgent.ParseAdd("my-refit-app");
        }).AddHttpMessageHandler<LoggingDelegatingHandler>();
    }

    public static void ConfigureWorkflows(ServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var host = serviceProvider.GetService<IWorkflowHost>();

        host.RegisterWorkflow<OrderWorkflow, OrderData>();
    }
}
