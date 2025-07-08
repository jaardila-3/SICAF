using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SICAF.Data.Context;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces;
using SICAF.Data.Repositories;
using SICAF.Data.Seeding;
using SICAF.Data.Services;

namespace SICAF.Data;

public static class ServiceRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Contexto de base de datos
        services.AddDbContext<SicafDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            options.UseMySQL(connectionString);
        });

        // Servicios
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IAuditable, User>();
        services.AddScoped<IAuditable, Role>();
        services.AddScoped<IAuditable, UserRole>();
        //services.AddScoped<ISpecification, >();

        // Repositorios
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Registrar seeders
        services.AddTransient<IDataSeeder, RoleSeeder>();
        services.AddTransient<IDataSeeder, UserSeeder>();

        // Registrar servicios de inicialización
        services.AddTransient<IDatabaseSetupService, DatabaseSetupService>();
        services.AddTransient<IDatabaseMigrationService, DatabaseMigrationService>();

        return services;
    }
}