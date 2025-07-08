using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using SICAF.Common.Constants;
using SICAF.Common.Enums;
using SICAF.Common.Interfaces;
using SICAF.Common.Models;
using SICAF.Data.Context;
using SICAF.Data.Entities.Common;
using SICAF.Data.Interfaces;

namespace SICAF.Data.Services;

public class AuditService(SicafDbContext context, IAuditContextProvider contextProvider) : IAuditService
{
    private readonly SicafDbContext _context = context;
    private readonly IAuditContextProvider _contextProvider = contextProvider;

    /// <summary>
    /// AUDITORÍA MANUAL: Se usa para registrar operaciones específicas
    /// Ejemplos: Login, Logout, Exportación de datos, Cambio de permisos
    /// </summary>
    public async Task<Guid> LogOperationAsync(
            string entityName,
            Guid entityId,
            AuditOperationType operationType,
            string? oldValues = null,
            string? newValues = null,
            string? description = null,
            AuditContext? customContext = null)
    {
        // Obtiene el contexto actual (usuario, IP, etc.) desde la capa Web
        var auditContext = customContext ?? _contextProvider.GetCurrentContext();

        var auditLog = new AuditLog
        {
            EntityName = entityName,
            EntityId = entityId,
            OperationType = operationType.ToString(),
            UserId = auditContext.UserId,
            UserName = auditContext.UserName,
            UserRole = auditContext.UserRole,
            IpAddress = auditContext.IpAddress,
            UserAgent = auditContext.UserAgent,
            SessionId = auditContext.SessionId,
            OldValues = oldValues,
            NewValues = newValues,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Set<AuditLog>().Add(auditLog);
        await _context.SaveChangesAsync();

        return auditLog.Id;
    }

    /// <summary>
    /// AUDITORÍA AUTOMÁTICA: Detecta y registra cambios en las entidades
    /// Se ejecuta automáticamente cuando se llama SaveChangesWithAuditAsync
    /// </summary>
    public async Task LogChangesAsync(SicafDbContext context)
    {
        var auditContext = _contextProvider.GetCurrentContext();

        // Obtiene todas las entidades que han cambiado
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity &&
                       (e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted))
            .ToList();

        var auditLogs = new List<AuditLog>();

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            // Manejo especial para SOFT DELETE
            if (entry.State == EntityState.Modified && entity.IsDeleted)
            {
                // Es un soft delete, cambiar el tipo de operación
                var auditLog = CreateAuditLog(entry, auditContext, AuditOperationType.Delete);
                auditLog.Description = AuditConstants.SoftDelete;
                auditLogs.Add(auditLog);
            }
            else
            {
                // Operación normal
                var auditLog = CreateAuditLog(entry, auditContext);
                auditLogs.Add(auditLog);
            }
        }

        if (auditLogs.Count != 0)
        {
            await context.Set<AuditLog>().AddRangeAsync(auditLogs);
        }
    }

    public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(
            Guid? entityId = null,
            string? entityName = null,
            string? userId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
    {
        var query = _context.Set<AuditLog>().AsQueryable();

        if (entityId.HasValue)
            query = query.Where(a => a.EntityId == entityId.Value);

        if (!string.IsNullOrEmpty(entityName))
            query = query.Where(a => a.EntityName == entityName);

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(a => a.UserId == userId);

        if (fromDate.HasValue)
            query = query.Where(a => a.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(a => a.CreatedAt <= toDate.Value);

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    #region private methods
    private string GetOperationType(EntityState state)
    {
        return state switch
        {
            EntityState.Added => AuditOperationType.Create.ToString(),
            EntityState.Modified => AuditOperationType.Update.ToString(),
            EntityState.Deleted => AuditOperationType.Delete.ToString(),
            _ => AuditConstants.UnknownValue
        };
    }

    private string GetEntityValues(EntityEntry entry)
    {
        var values = entry.CurrentValues.Properties
            .ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
        return JsonSerializer.Serialize(values);
    }

    private string GetOriginalValues(EntityEntry entry)
    {
        var values = entry.OriginalValues.Properties
            .ToDictionary(p => p.Name, p => entry.OriginalValues[p]);
        return JsonSerializer.Serialize(values);
    }

    private string GetCurrentValues(EntityEntry entry)
    {
        var values = entry.CurrentValues.Properties
            .ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
        return JsonSerializer.Serialize(values);
    }

    private string GetChangedProperties(EntityEntry entry)
    {
        var modifiedProperties = entry.Properties
            .Where(p => p.IsModified)
            .Select(p => p.Metadata.Name)
            .ToList();
        return JsonSerializer.Serialize(modifiedProperties);
    }

    // <summary>
    /// Crea un log de auditoría para una entrada de EF Core
    /// </summary>
    private AuditLog CreateAuditLog(EntityEntry entry, AuditContext auditContext, AuditOperationType? overrideType = null)
    {
        var entity = (BaseEntity)entry.Entity;
        var operationType = overrideType?.ToString() ?? GetOperationType(entry.State);

        var auditLog = new AuditLog
        {
            EntityName = entry.Entity.GetType().Name,
            EntityId = entity.Id,
            OperationType = operationType,
            UserId = auditContext.UserId,
            UserName = auditContext.UserName,
            UserRole = auditContext.UserRole,
            IpAddress = auditContext.IpAddress,
            UserAgent = auditContext.UserAgent,
            SessionId = auditContext.SessionId,
            CreatedAt = DateTime.UtcNow
        };

        // Si la entidad implementa IAuditable, obtener información adicional
        if (entry.Entity is IAuditable auditable)
        {
            auditLog.Description = auditable.GetAuditDescription();
            auditLog.Module = auditable.GetModule();
            auditLog.SubModule = auditable.GetSubModule();
        }

        // Capturar valores según el tipo de operación
        switch (entry.State)
        {
            case EntityState.Added:
                auditLog.NewValues = GetEntityValues(entry);
                break;

            case EntityState.Modified:
                auditLog.OldValues = GetOriginalValues(entry);
                auditLog.NewValues = GetCurrentValues(entry);
                auditLog.ChangedProperties = GetChangedProperties(entry);
                break;

            case EntityState.Deleted:
                auditLog.OldValues = GetOriginalValues(entry);
                break;
        }

        return auditLog;
    }
    #endregion
}