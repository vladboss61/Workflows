using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dependency.ConsoleApp;

public interface IServiceA
{
    
}

public class ServiceA : IServiceA
{
    public ServiceA(IServiceB serviceB)
    {

    }
}

public interface IServiceB
{
    public void Example()
    {
        Console.WriteLine("Example");
    }
}

public class ServiceB : IServiceB
{

}

internal sealed class Program
{
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IServiceA, ServiceA>();
        serviceCollection.AddScoped<IServiceB, ServiceB>();

        var serviceProvider = serviceCollection.BuildServiceProvider(
            new ServiceProviderOptions
            {
                // During BuildServiceProvider call, the ValidateOnBuild = true ensures that the service provider is validated at build time.
                // And get errors immediately if there are any issues with the service registrations. Comment out the line below to see the difference.
                // And comment serviceCollection.AddScoped<IServiceB, ServiceB>(); and add IServiceB to ServiceA constructor to see the error.
                //ValidateOnBuild = true,
                ValidateScopes = true
            });

        try
        {
            serviceProvider.GetService<IServiceA>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during resolving from root: {ex.Message}");
        }

        using (var scope = serviceProvider.CreateScope())
        {
            var serviceB = scope.ServiceProvider.GetService<IServiceB>();
            serviceB.Example();
        }

        Console.WriteLine("Hello, World!");
    }
}
