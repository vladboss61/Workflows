using HostCli.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Exceptions;
using System;
using System.IO;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace HostCli;

public class Program
{
    static async Task Main(string[] args)
    {
        string env = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? Environments.Production;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithMachineName()
            .Enrich.WithProperty("ApplicationName", "Ex CLI App")
            .Enrich.WithProperty("ApplicationVersion", "1.0.0")
            .Enrich.WithExceptionDetails()
            .Enrich.FromLogContext()
            .CreateLogger();

        CancellationTokenSource cancellationTokenSource = PrepareConsoleCancellationTokenSource();

        try
        {
            Log.Information("Starting up application.");

            var host = Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment(env)
                .UseSerilog(Log.Logger)
                //.UseConsoleLifetime()
                .ConfigureAppConfiguration(builder =>
                {
                    // Not required but okey - Clear().
                    builder.Sources.Clear();
                    builder.AddConfiguration(configuration);
                })
                .ConfigureServices(async (context, services) =>
                {
                    Console.WriteLine(context.HostingEnvironment.EnvironmentName);
                    await new Startup(configuration, Log.Logger).ConfigureServices(services);
                })
                .Build();

            var app = host.Services.GetRequiredService<Application>();
            await app.RunAsync(cancellationTokenSource.Token);

            Log.Information("Shutting down application");
        }
        catch (TaskCanceledException taskEx)
        {
            Log.Warning(taskEx, "Application canceled.");
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

    private static CancellationTokenSource PrepareConsoleCancellationTokenSource()
    {
        var cancellationTokenSource = new CancellationTokenSource();

        //ConsoleCancelEventHandler cancelKeyPressSubscriber = (sender, e) =>
        //{
        //    Log.Logger.Information("Ctrl+C is executed and operation is cancelled");
        //    e.Cancel = true;
        //    cancellationTokenSource.Cancel();
        //};

        // Cancel on Ctrl+C or SIGTERM
        //Console.CancelKeyPress += cancelKeyPressSubscriber;
        AssemblyLoadContext.Default.Unloading += ctx => cancellationTokenSource.Cancel();

        return cancellationTokenSource;
    }
}
