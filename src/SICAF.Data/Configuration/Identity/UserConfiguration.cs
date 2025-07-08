using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Common.Enums;
using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Configuration.Identity;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        // CONFIGURACIÓN DE TABLA
        builder.ToTable("Users");

        // CONFIGURACIÓN DE PROPIEDADES REQUERIDAS
        builder.Property(u => u.DocumentType)
            .IsRequired()
            .HasMaxLength(10)
            .HasComment("Tipo de documento de identificación (CC, TI, CE, etc.)");

        builder.Property(u => u.IdentificationNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Número del documento de identificación");

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Nombres del usuario");

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Apellidos del usuario");

        builder.Property(u => u.Nationality)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Colombia")
            .HasComment("Nacionalidad del usuario");

        builder.Property(u => u.BloodType)
            .IsRequired()
            .HasMaxLength(5)
            .HasComment("Factor RH de sangre (O+, O-, A+, A-, B+, B-, AB+, AB-)");

        builder.Property(u => u.BirthDate)
            .IsRequired()
            .HasColumnType("date")
            .HasComment("Fecha de nacimiento del usuario");

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Nombre de usuario único para login");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Correo electrónico del usuario");

        // CONFIGURACIÓN DE PROPIEDADES OPCIONALES
        builder.Property(u => u.Grade)
            .HasMaxLength(50)
            .HasComment("Grado policial del usuario");

        builder.Property(u => u.Force)
            .HasMaxLength(100)
            .HasComment("Fuerza a la que pertenece (Policía Nacional, Ejercito, etc.)");

        builder.Property(u => u.Photo)
            .HasColumnType("longtext")
            .HasComment("Fotografía del usuario en Base64");

        builder.Property(u => u.Dependency)
            .HasMaxLength(100)
            .HasComment("Dependencia o unidad a la que pertenece");

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(500)
            .HasComment("Hash de la contraseña del usuario");

        builder.Property(u => u.LockoutReason)
            .HasMaxLength(500)
            .HasComment("Razón del bloqueo del usuario");

        // CONFIGURACIÓN DE ENUMS
        builder.Property(u => u.UserType)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(UserType.Student)
            .HasComment("Tipo de usuario en el sistema");

        // CONFIGURACIÓN DE CAMPOS DE FECHA/HORA
        builder.Property(u => u.LockoutEnd)
            .HasColumnType("datetime")
            .HasComment("Fecha y hora hasta cuando el usuario está bloqueado");

        // CONFIGURACIÓN DE CAMPOS NUMÉRICOS
        builder.Property(u => u.AccessFailedCount)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Número de intentos fallidos de autenticación");

        // CONFIGURACIÓN DE ÍNDICES ÚNICOS
        // Índice único compuesto para documento
        builder.HasIndex(u => new { u.DocumentType, u.IdentificationNumber })
            .IsUnique()
            .HasDatabaseName("IX_Users_Document_Unique")
            .HasFilter("IsDeleted = FALSE"); // Solo para registros no eliminados

        // Índice único para nombre de usuario
        builder.HasIndex(u => u.Username)
            .IsUnique()
            .HasDatabaseName("IX_Users_Username_Unique")
            .HasFilter("IsDeleted = FALSE");

        // Índice único para email
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email_Unique")
            .HasFilter("IsDeleted = FALSE");

        // ÍNDICES DE RENDIMIENTO
        // Índice para búsquedas por nombre completo
        builder.HasIndex(u => new { u.Name, u.LastName })
            .HasDatabaseName("IX_Users_FullName");

        // Índice para tipo de usuario
        builder.HasIndex(u => u.UserType)
            .HasDatabaseName("IX_Users_UserType");

        // Índice para fecha de nacimiento
        builder.HasIndex(u => u.BirthDate)
            .HasDatabaseName("IX_Users_BirthDate");

        // Índice para dependencia
        builder.HasIndex(u => u.Dependency)
            .HasDatabaseName("IX_Users_Dependency");

        // Índice para usuarios bloqueados
        builder.HasIndex(u => u.LockoutEnd)
            .HasDatabaseName("IX_Users_LockoutEnd");

        // CONFIGURACIÓN DE RELACIONES
        // Relación uno a muchos con UserRoles
        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserRoles_Users");
    }
}