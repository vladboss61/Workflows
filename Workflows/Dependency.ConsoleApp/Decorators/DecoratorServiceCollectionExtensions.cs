using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Dependency.ConsoleApp.Decorators;

public static class DecoratorServiceCollectionExtensions
{
    public static IServiceCollection AddSingletonDecorator<TService, TImplementation, TDecorator1>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        where TDecorator1 : class, TService
    {
        return services.AddDecorator<TService, TImplementation, TDecorator1>(ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddScopedDecorator<TService, TImplementation, TDecorator1>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        where TDecorator1 : class, TService
    {
        return services.AddDecorator<TService, TImplementation, TDecorator1>(ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddTransientDecorator<TService, TImplementation, TDecorator1>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        where TDecorator1 : class, TService
    {
        return services.AddDecorator<TService, TImplementation, TDecorator1>(ServiceLifetime.Transient);
    }

    public static IServiceCollection AddSingletonDecorator<TService, TImplementation, TDecorator1, TDecorator2>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        where TDecorator1 : class, TService
        where TDecorator2 : class, TService
    {
        return services.AddDecorator<TService, TImplementation, TDecorator1, TDecorator2>(ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddScopedDecorator<TService, TImplementation, TDecorator1, TDecorator2>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        where TDecorator1 : class, TService
        where TDecorator2 : class, TService
    {
        return services.AddDecorator<TService, TImplementation, TDecorator1>(ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddTransientDecorator<TService, TImplementation, TDecorator1, TDecorator2>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        where TDecorator1 : class, TService
        where TDecorator2 : class, TService
    {
        return services.AddDecorator<TService, TImplementation, TDecorator1, TDecorator2>(ServiceLifetime.Transient);
    }

    public static IServiceCollection AddDecorator<TService, TImplementation, TDecorator1>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
                where TService : class
                where TImplementation : class, TService
                where TDecorator1 : class, TService
    {
        services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), lifetime));

        services.Add(
            new ServiceDescriptor(
                typeof(TService),
                serviceProvider =>
                    ActivatorUtilities.CreateInstance<TDecorator1>(
                        serviceProvider,
                        serviceProvider.GetRequiredService<TImplementation>()),
                lifetime));

        return services;
    }

    public static IServiceCollection AddDecorator<TService, TImplementation, TDecorator1, TDecorator2>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TService : class
            where TImplementation : class, TService
            where TDecorator1 : class, TService
            where TDecorator2 : class, TService
    {
        services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), lifetime));

        services.Add(
            new ServiceDescriptor(
            typeof(TService),
                serviceProvider => ActivatorUtilities.CreateInstance<TDecorator2>(
                    serviceProvider, ActivatorUtilities.CreateInstance<TDecorator1>(
                        serviceProvider, serviceProvider.GetRequiredService<TImplementation>())),
                lifetime));

        return services;
    }
}
