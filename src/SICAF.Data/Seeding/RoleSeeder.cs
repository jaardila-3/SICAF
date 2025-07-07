using Microsoft.Extensions.Logging;

using SICAF.Common.Constants;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces;

namespace SICAF.Data.Seeding;

public class RoleSeeder(IUnitOfWork unitOfWork, ILogger<RoleSeeder> logger) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<RoleSeeder> _logger = logger;

    public int Priority => 1; // Alta prioridad - los roles deben crearse primero

    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando seeding de roles...");

        var roles = GetDefaultRoles();

        foreach (var role in roles)
        {
            await SeedRoleAsync(role);
        }

        _logger.LogInformation("Seeding de roles completado.");
    }

    private async Task SeedRoleAsync(Role role)
    {
        var existingRole = await _unitOfWork.Repository<Role>()
            .GetFirstAsync(r => r.Name.Equals(role.Name));

        if (existingRole == null)
        {
            await _unitOfWork.Repository<Role>().AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Rol {RoleName} creado exitosamente.", role.Name);
        }
        else
        {
            _logger.LogInformation("Rol {RoleName} ya existe.", role.Name);
        }
    }

    private static List<Role> GetDefaultRoles()
    {
        return
        [
            new()
            {
                Name = RoleNames.ADMIN,
                Description = "Administrador del sistema con acceso completo"
            },
            new()
            {
                Name = RoleNames.ACADEMIC_ADMIN,
                Description = "Administrador académico con gestión de cursos y estudiantes"
            },
            new()
            {
                Name = RoleNames.USERS_ADMIN,
                Description = "Administrador de usuarios con gestión de cuentas y roles"
            },
            new()
            {
                Name = RoleNames.ACADEMIC_MONITOR,
                Description = "Monitor y seguimiento académico con acceso limitado"
            },
            new()
            {
                Name = RoleNames.INSTRUCTOR,
                Description = "Instructor de vuelo con capacidad de evaluación"
            },
            new()
            {
                Name = RoleNames.STUDENT,
                Description = "Estudiante del programa de aviación policial"
            },
            new()
            {
                Name = RoleNames.FLIGHT_LEADER,
                Description = "Líder de vuelo con supervisión académica especializada"
            }
        ];
    }
}