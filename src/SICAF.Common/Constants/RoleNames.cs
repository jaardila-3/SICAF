namespace SICAF.Common.Constants;

/// <summary>
/// Constantes para los nombres de roles del sistema SICAF
/// </summary>
public static class RoleNames
{
    /// <summary>
    /// Administrador del sistema con acceso completo
    /// </summary>
    public const string ADMIN = "Administrador";

    /// <summary>
    /// Administrador de usuarios del sistema
    /// </summary>
    public const string USERS_ADMIN = "Administrador_Usuarios";

    /// <summary>
    /// Administrador académico con gestión de cursos y estudiantes
    /// </summary>
    public const string ACADEMIC_ADMIN = "Administrador_Academico";

    /// <summary>
    /// Seguimiento académico con gestión de notas e informes
    /// </summary>
    public const string ACADEMIC_MONITOR = "Seguimiento_Academico";

    /// <summary>
    /// Instructor de vuelo con capacidad de evaluación
    /// </summary>
    public const string INSTRUCTOR = "Instructor_Vuelo";

    /// <summary>
    /// Estudiante del programa de aviación policial
    /// </summary>
    public const string STUDENT = "Estudiante";

    /// <summary>
    /// Líder de vuelo con supervisión académica especializada
    /// </summary>
    public const string FLIGHT_LEADER = "Líder_vuelo";

    /// <summary>
    /// Obtiene todos los roles disponibles
    /// </summary>
    public static IReadOnlyList<string> All => new[]
    {
        ADMIN,
        ACADEMIC_ADMIN,
        INSTRUCTOR,
        STUDENT,
        FLIGHT_LEADER
    };
}
