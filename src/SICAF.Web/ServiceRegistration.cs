using Microsoft.AspNetCore.Authentication.Cookies;

using SICAF.Common.Interfaces;
using SICAF.Web.Services;

namespace SICAF.Web;

public static class ServiceRegistration
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        // Servicios específicos de la capa Web
        services.AddHttpContextAccessor();
        services.AddScoped<IAuditContextProvider, HttpAuditContextProvider>();

        // Configuración de sesión, cookies, etc.
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        //configuration auth and cookies
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Auth/Login";
                options.AccessDeniedPath = "/Account/Auth/AccessDenied";
                options.LogoutPath = "/Account/Auth/Logout";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Name = "WebAppCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });

        // configure the application routes to use lowercase urls
        services.AddRouting(options => options.LowercaseUrls = true);

        return services;
    }
}