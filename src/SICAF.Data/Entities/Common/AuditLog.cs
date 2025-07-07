using System.ComponentModel.DataAnnotations;

namespace SICAF.Data.Entities.Common;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Información de la entidad
    [Required]
    [MaxLength(100)]
    public string EntityName { get; set; } = string.Empty;

    [Required]
    public Guid EntityId { get; set; }

    // Tipo de operación
    [Required]
    [MaxLength(50)]
    public string OperationType { get; set; } = string.Empty; // Create, Update, Delete, Read

    // Información del usuario
    [Required]
    [MaxLength(100)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string UserRole { get; set; } = string.Empty;

    // Información de la sesión
    [MaxLength(50)]
    public string IpAddress { get; set; } = string.Empty;

    [MaxLength(500)]
    public string UserAgent { get; set; } = string.Empty;

    [MaxLength(100)]
    public string SessionId { get; set; } = string.Empty;

    // Datos de la operación
    public string? OldValues { get; set; } // JSON con valores anteriores
    public string? NewValues { get; set; } // JSON con valores nuevos
    public string? ChangedProperties { get; set; } // Lista de propiedades modificadas

    // Información adicional
    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Module { get; set; }

    [MaxLength(100)]
    public string? SubModule { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Estado de la operación
    public bool IsSuccessful { get; set; } = true;

    [MaxLength(1000)]
    public string? ErrorMessage { get; set; }
}