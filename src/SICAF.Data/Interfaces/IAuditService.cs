using SICAF.Common.Enums;
using SICAF.Common.Models;
using SICAF.Data.Context;
using SICAF.Data.Entities.Common;

namespace SICAF.Data.Interfaces;

public interface IAuditService
{
    Task<Guid> LogOperationAsync(
            string entityName,
            Guid entityId,
            AuditOperationType operationType,
            string? oldValues = null,
            string? newValues = null,
            string? description = null,
            AuditContext? customContext = null);

    Task LogChangesAsync(SicafDbContext context);

    Task<IEnumerable<AuditLog>> GetAuditLogsAsync(
        Guid? entityId = null,
        string? entityName = null,
        string? userId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null);
}