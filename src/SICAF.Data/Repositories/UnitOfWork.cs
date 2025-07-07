using System.Collections;

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

using SICAF.Data.Context;
using SICAF.Data.Entities.Common;
using SICAF.Data.Interfaces;

namespace SICAF.Data.Repositories;

public class UnitOfWork(
    SicafDbContext context,
    ILogger<UnitOfWork> logger,
    IAuditService? auditService = null) : IUnitOfWork
{
    private readonly Hashtable _repositories = [];
    private readonly SicafDbContext _context = context;
    private readonly ILogger<UnitOfWork> _logger = logger;
    private readonly IAuditService? _auditService = auditService;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    /// <summary>
    /// Obtiene un repositorio genérico para cualquier entidad
    /// </summary>
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        var typeName = type.Name;

        if (!_repositories.ContainsKey(typeName))
        {
            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(type), _context);

            _repositories[typeName] = repositoryInstance;
        }

        return (IRepository<T>)_repositories[typeName]!;
    }

    /// <summary>
    /// Guarda los cambios sin auditoría
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar cambios en UnitOfWork");
            throw new Exception($"Error al guardar cambios: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Guarda los cambios con auditoría automática
    /// </summary>
    public async Task<int> SaveChangesWithAuditAsync()
    {
        try
        {
            if (_auditService != null)
            {
                await _auditService.LogChangesAsync(_context);
            }

            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar cambios con auditoría en UnitOfWork");
            throw new Exception($"Error al guardar cambios con auditoría: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Inicia una transacción (MySQL soporta transacciones)
    /// </summary>
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        try
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar transacción");
            throw;
        }
    }

    /// <summary>
    /// Confirma la transacción actual
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Deshace la transacción actual
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// Ejecuta múltiples operaciones dentro de una transacción
    /// </summary>
    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
    {
        // Si ya hay una transacción activa, usar esa
        if (_transaction != null)
        {
            return await operation();
        }

        // Crear nueva transacción
        using var transaction = await BeginTransactionAsync();
        try
        {
            var result = await operation();
            await CommitTransactionAsync();
            return result;
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}