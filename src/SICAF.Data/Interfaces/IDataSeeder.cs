namespace SICAF.Data.Interfaces;

/// <summary>
/// Interface común para todas las clases de seeding de datos
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    /// Ejecuta el proceso de seeding de datos
    /// </summary>
    /// <returns>Tarea asíncrona</returns>
    Task SeedAsync();

    /// <summary>
    /// Obtiene la prioridad de ejecución del seeder (menor número = mayor prioridad)
    /// </summary>
    int Priority { get; }
}