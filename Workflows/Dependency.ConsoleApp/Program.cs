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

internal class Program
{
    public static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IServiceA, ServiceA>();
        serviceCollection.AddScoped<IServiceB, ServiceB>();

        var serviceProvider = serviceCollection.BuildServiceProvider(
            new ServiceProviderOptions
            {
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
