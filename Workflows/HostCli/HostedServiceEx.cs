using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HostCli;

internal sealed class HostedServiceEx : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while(!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Hosted Service is running...");
            await Task.Delay(2000);
        }
        Console.WriteLine("Finished Loop.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopped");
        return Task.CompletedTask;
    }
}
