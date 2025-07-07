using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SICAF.Common.Constants;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces;

namespace SICAF.Data.Seeding;

public class DatabaseSetupService(
    IServiceProvider serviceProvider,
    ILogger<DatabaseSetupService> logger,
    IUnitOfWork unitOfWork) : IDatabaseSetupService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<DatabaseSetupService> _logger = logger;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Ejecuta todos los seeders en orden de prioridad
    /// </summary>
    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando inicialización de datos del sistema SICAF...");

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>()
                .OrderBy(s => s.Priority)
                .ToList();

            _logger.LogInformation("Se encontraron {SeederCount} seeders para ejecutar.", seeders.Count);

            foreach (var seeder in seeders)
            {
                var seederName = seeder.GetType().Name;
                _logger.LogInformation("Ejecutando seeder: {SeederName} (Prioridad: {Priority})",
                    seederName, seeder.Priority);

                await seeder.SeedAsync();

                _logger.LogInformation("Seeder {SeederName} ejecutado exitosamente.", seederName);
            }

            _logger.LogInformation("Inicialización de datos completada exitosamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la inicialización de datos.");
            throw;
        }
    }

    /// <summary>
    /// Verifica si hay datos existentes antes de ejecutar seeding
    /// </summary>
    public async Task<bool> HasInitialDataAsync()
    {
        try
        {
            var hasRoles = await _unitOfWork.Repository<Role>().AnyAsync(r => r.Name == RoleNames.ADMIN);
            var hasUsers = await _unitOfWork.Repository<User>().AnyAsync(u => u.Email != null);
            var hasUserRoles = await _unitOfWork.Repository<UserRole>().AnyAsync(ur => ur.Role.Name == RoleNames.ADMIN);

            return hasRoles && hasUsers && hasUserRoles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando datos iniciales.");
            return false;
        }
    }
}