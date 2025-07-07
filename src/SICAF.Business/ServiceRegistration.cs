using Microsoft.Extensions.DependencyInjection;

using SICAF.Business.DomainServices.Implementation;
using SICAF.Data.Interfaces;

namespace SICAF.Business;

public static class ServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Servicios de negocio
        services.AddScoped<IAuditService, AuditService>();
        //services.AddScoped<IAuditable, >();

        return services;
    }
}