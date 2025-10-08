using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDIExample.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWithLogging<TInterface, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.Add(new ServiceDescriptor(
            typeof(TImplementation),
            typeof(TImplementation),
            lifetime
        ));

        services.Add(new ServiceDescriptor(
            typeof(TInterface),
            provider =>
            {
                var implementation = provider.GetRequiredService<TImplementation>();
                var logger = provider.GetRequiredService<ILogger<TImplementation>>();
                return LoggingInterceptor<TInterface>.Create(implementation, logger);
            },
            lifetime
        ));

        return services;
    }
}
