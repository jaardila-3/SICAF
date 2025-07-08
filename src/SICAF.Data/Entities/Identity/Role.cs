using SICAF.Data.Entities.Common;
using SICAF.Data.Interfaces;

namespace SICAF.Data.Entities.Identity;

public class Role : BaseEntity, IAuditable
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // relationships
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];

    // Implementación de IAuditable
    public string GetAuditDescription()
    {
        return $"Rol: {Name}";
    }

    public string GetModule()
    {
        return "Account";
    }

    public string GetSubModule()
    {
        return "Roles";
    }
}