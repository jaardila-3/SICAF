using SICAF.Data.Entities.Common;
using SICAF.Data.Interfaces;

namespace SICAF.Data.Entities.Identity;

public class User : BaseEntity, IAuditable
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Dependency { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public string? LockoutReason { get; set; }
    public int AccessFailedCount { get; set; }

    // relationships
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];

    // Implementación de IAuditable
    public string GetAuditDescription()
    {
        return $"Usuario: {Username} ({Name} {LastName})";
    }

    public string GetModule()
    {
        return "Account";
    }

    public string GetSubModule()
    {
        return "Users";
    }
}