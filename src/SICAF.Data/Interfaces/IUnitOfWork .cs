using Microsoft.EntityFrameworkCore.Storage;

using SICAF.Data.Entities.Common;

namespace SICAF.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Obtiene un repositorio genérico para cualquier entidad
    /// </summary>
    IRepository<T> Repository<T>() where T : BaseEntity;

    /// <summary>
    /// Guarda los cambios sin auditoría
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Guarda los cambios con auditoría automática
    /// </summary>
    Task<int> SaveChangesWithAuditAsync();

    /// <summary>
    /// Manejo de transacciones
    /// </summary>
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

    /// <summary>
    /// Ejecuta una operación dentro de una transacción
    /// </summary>
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
}