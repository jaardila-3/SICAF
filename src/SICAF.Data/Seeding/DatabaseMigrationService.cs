using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SICAF.Data.Context;
using SICAF.Data.Interfaces;

namespace SICAF.Data.Seeding;

public class DatabaseMigrationService(SicafDbContext context, ILogger<DatabaseMigrationService> logger) : IDatabaseMigrationService
{
    private readonly SicafDbContext _context = context;
    private readonly ILogger<DatabaseMigrationService> _logger = logger;

    /// <summary>
    /// Ejecuta las migraciones pendientes de la base de datos
    /// </summary>
    public async Task MigrateDatabaseAsync()
    {
        _logger.LogInformation("Iniciando migración de base de datos...");

        try
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            var pendingCount = pendingMigrations.Count();

            if (pendingCount > 0)
            {
                _logger.LogInformation("Se encontraron {PendingCount} migraciones pendientes.", pendingCount);

                foreach (var migration in pendingMigrations)
                {
                    _logger.LogInformation("Migración pendiente: {MigrationName}", migration);
                }

                await _context.Database.MigrateAsync();
                _logger.LogInformation("Migraciones aplicadas exitosamente.");
            }
            else
            {
                _logger.LogInformation("No hay migraciones pendientes.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la migración de la base de datos.");
            throw;
        }
    }

    /// <summary>
    /// Verifica si la base de datos puede conectarse
    /// </summary>
    public async Task<bool> CanConnectAsync()
    {
        try
        {
            return await _context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "No se puede conectar a la base de datos.");
            return false;
        }
    }

}