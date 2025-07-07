namespace SICAF.Data.Interfaces;

public interface IDatabaseMigrationService
{
    /// <summary>
    /// Ejecuta las migraciones pendientes de la base de datos
    /// </summary>
    /// <returns>Tarea asíncrona</returns>
    Task MigrateDatabaseAsync();

    /// <summary>
    /// Verifica si la base de datos puede conectarse
    /// </summary>
    /// <returns>True si puede conectarse, false en caso contrario</returns>
    Task<bool> CanConnectAsync();

}