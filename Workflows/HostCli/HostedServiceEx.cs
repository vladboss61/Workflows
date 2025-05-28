using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostCli;

internal sealed class HostedServiceEx : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var random = new Random().Next(0, 300);
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"Hosted Service {random} is running...");
            await Task.Delay(2000, cancellationToken);
        }
        Console.WriteLine("Finished Loop.");
    }
}

internal sealed class HostedServiceEx2 : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var random = new Random().Next(0, 300);
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine($"Hosted Service 2 {random} is running...");
            await Task.Delay(2000, cancellationToken);
        }
        Console.WriteLine("Finished Loop.");
    }
}
