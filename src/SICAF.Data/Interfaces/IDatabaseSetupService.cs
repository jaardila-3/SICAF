namespace SICAF.Data.Interfaces;

public interface IDatabaseSetupService
{
    /// <summary>
    /// Ejecuta todos los seeders en orden de prioridad
    /// </summary>
    /// <returns>Tarea asíncrona</returns>
    Task SeedAsync();

    /// <summary>
    /// Verifica si hay datos existentes antes de ejecutar seeding
    /// </summary>
    /// <returns>True si la base de datos ya tiene datos iniciales</returns>
    Task<bool> HasInitialDataAsync();
}