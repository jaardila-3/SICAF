using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SICAF.Common.AppConfiguration;
using SICAF.Common.Implementations;
using SICAF.Common.Interfaces;

namespace SICAF.Common;

public static class ServiceRegistration
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
    {
        //configure DI for services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        // Configure specific settings
        services.AddOptions<AdminSettings>()
        .Bind(configuration.GetSection(AdminSettings.SectionName))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        return services;
    }
}