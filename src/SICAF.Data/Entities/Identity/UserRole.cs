using SICAF.Common.Constants;
using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Identity;

public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime? ExpiresAt { get; set; } // Para roles temporales

    // relationships
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;

    // Implementación de IAuditable
    public string GetAuditDescription()
    {
        // Descripción detallada de la asignación
        var description = $"Asignación de rol '{Role?.Name ?? AuditConstants.UnknownValue}' " +
                        $"al usuario '{User?.Username ?? AuditConstants.UnknownValue}'";

        if (ExpiresAt.HasValue)
        {
            description += $" (Expira: {ExpiresAt.Value:yyyy-MM-dd})";
        }

        return description;
    }

    public string GetModule()
    {
        return "Account";
    }

    public string GetSubModule()
    {
        return "UserRoles";
    }
}