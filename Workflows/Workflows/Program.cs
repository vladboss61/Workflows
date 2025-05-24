namespace WorkflowsEx;

using System;
using System.IO;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowsEx.GithubApi;
using WorkflowsEx.Workflows;
using WorkflowsEx.Infrastructure;
using WorkflowsEx.Workflows.Data;
using WorkflowsEx.DogsApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Refit;
using System.Net.Http.Headers;
using System.Threading;
using System.Net.Mime;
using System.Net.Http;
using Polly;
using Polly.Timeout;
using Polly.Extensions.Http;
using Polly.CircuitBreaker;
using Polly.Retry;

public sealed class Program
{
    private static ILogger<Application> _logger;

    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        ServiceCollection services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<ConfigurationSettings>(configuration);
        
        var settings = services.BuildServiceProvider().GetRequiredService<IOptions<ConfigurationSettings>>().Value;
        
        services.AddTransient<LoggingDelegatingHandler>();

        AddRemoteRefitClient<IGithubRepository>(services, settings.GithubUrl);
        AddRemoteRefitClient<IDogsRepository>(services, settings.DogsUrl);

        // Default Microsoft Logger.
        services.AddLogging(builder =>
        {
            // Optional: filter out low-level logs
            // We can read in from appsettings.json or configure here.
            builder
                .AddFilter("Default", LogLevel.Information)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddConsole();
        });

        _logger = services.BuildServiceProvider().GetRequiredService<ILogger<Application>>();

        services.AddWorkflow(); // in-memory persistence

        ConfigureWorkflows(services);

        services.AddSingleton<Application>();
        await services.BuildServiceProvider().GetRequiredService<Application>().RunAsync();
    }

    private static void AddRemoteRefitClient<T>(ServiceCollection services, string baseAddressUrl)
        where T : class
    {
        services.AddRefitClient<T>().ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(baseAddressUrl);
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("corporation-my-refit-app", "1.0.0"));

        }).AddHttpMessageHandler<LoggingDelegatingHandler>()
          .AddPolicyHandler(GetTimeoutPolicy())
          .AddPolicyHandler(GetCircuitBreakerPolicy())
          .AddPolicyHandler(GetRetryPolicy()); ;
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                attempt => TimeSpan.FromMilliseconds(
                    (1 << attempt) * 1000 +
                    DateTime.UtcNow.Ticks % 2000),
                static (responseMessage, _, retryNumber, _) => _logger.LogWarning($"Polly retry number ${retryNumber}"));
    }

    private static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TaskCanceledException>()
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 3, TimeSpan.FromMilliseconds(60000));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy
            .TimeoutAsync(
                timeout: TimeSpan.FromMilliseconds(30000),
                TimeoutStrategy.Optimistic,
                (_, _, _, exception) => Task.FromException(new TimeoutException("The HTTP request has timed out.", exception)))
            .AsAsyncPolicy<HttpResponseMessage>();
    }

    public static void ConfigureWorkflows(ServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var host = serviceProvider.GetService<IWorkflowHost>();

        host.RegisterWorkflow<OrderWorkflow, OrderData>();
    }
}
