namespace SICAF.Common.Constants;

public static class AuditConstants
{
    public const string SystemUser = "SISTEMA";
    public const string AnonymousUser = "ANONIMO";
    public const string UnknownValue = "DESCONOCIDO";
    public const string SoftDelete = "ELIMINADO LOGICO";

    public static class Claims
    {
        public const string FullName = "FullName";
        public const string Role = "Role";
        public const string UserId = "UserId";
    }

    public static class Headers
    {
        public const string UserAgent = "User-Agent";
        public const string ForwardedFor = "X-Forwarded-For";
    }
}