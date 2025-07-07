using SICAF.Common.Constants;
using SICAF.Common.Interfaces;
using SICAF.Common.Models;

namespace SICAF.Web.Services;

public class HttpAuditContextProvider : IAuditContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpAuditContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public AuditContext GetCurrentContext()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            return new AuditContext();
        }

        return new AuditContext
        {
            UserId = httpContext.User?.Identity?.Name ?? AuditConstants.AnonymousUser,
            UserName = httpContext.User?.Claims?
                .FirstOrDefault(c => c.Type == AuditConstants.Claims.FullName)?.Value
                      ?? httpContext.User?.Identity?.Name ?? AuditConstants.AnonymousUser,
            UserRole = httpContext.User?.Claims?.FirstOrDefault(c => c.Type == AuditConstants.Claims.Role)?.Value ?? AuditConstants.AnonymousUser,
            IpAddress = GetClientIpAddress(httpContext),
            UserAgent = httpContext.Request.Headers[AuditConstants.Headers.UserAgent].ToString(),
            SessionId = httpContext.Session?.Id ?? string.Empty
        };
    }

    private string GetClientIpAddress(HttpContext context)
    {
        // Verificar si está detrás de un proxy
        var forwarded = context.Request.Headers[AuditConstants.Headers.ForwardedFor]
            .FirstOrDefault();
        if (!string.IsNullOrEmpty(forwarded))
        {
            return forwarded.Split(',').First().Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? AuditConstants.UnknownValue;
    }
}