using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Entities.Common;

namespace SICAF.Data.Configuration.Common;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(e => e.Id);

        // Índices para mejorar rendimiento
        builder.HasIndex(e => e.EntityId);
        builder.HasIndex(e => e.EntityName);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => new { e.EntityName, e.EntityId });
        builder.HasIndex(e => new { e.Module, e.SubModule });

        // Configuración de propiedades JSON
        builder.Property(e => e.OldValues)
            .HasColumnType("JSON");

        builder.Property(e => e.NewValues)
            .HasColumnType("JSON");

        builder.Property(e => e.ChangedProperties)
            .HasColumnType("JSON");
    }
}