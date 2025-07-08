using System.ComponentModel.DataAnnotations.Schema;

using SICAF.Common.Enums;
using SICAF.Data.Entities.Common;
using SICAF.Data.Interfaces;

namespace SICAF.Data.Entities.Identity;

public class User : BaseEntity, IAuditable
{
    public string DocumentType { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string? Grade { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty; //RH
    public DateTime BirthDate { get; set; }
    public string? Force { get; set; }
    public UserType UserType { get; set; } = UserType.Student;
    public string? Photo { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Dependency { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public string? LockoutReason { get; set; }
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    [NotMapped]
    public string FullName => $"{Name} {LastName}".Trim();

    /// <summary>
    /// Identificación completa (Tipo + Número)
    /// </summary>
    [NotMapped]
    public string FullIdentification => $"{DocumentType}: {IdentificationNumber}";

    /// <summary>
    /// Indica si el usuario está actualmente bloqueado
    /// </summary>
    [NotMapped]
    public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;

    /// <summary>
    /// Edad del usuario basada en la fecha de nacimiento
    /// </summary>
    [NotMapped]
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

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