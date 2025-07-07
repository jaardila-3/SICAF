using System.Globalization;

using DotNetEnv;

using Microsoft.AspNetCore.Localization;

using Serilog;

using SICAF.Business;
using SICAF.Common;
using SICAF.Data;
using SICAF.Data.Interfaces;
using SICAF.Web;

// Load environment variables from .env file
Env.Load();

// Configure culture info for the application
var cultureInfo = new CultureInfo("es-CO");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure localization options
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = [cultureInfo],
    SupportedUICultures = [cultureInfo]
};

var builder = WebApplication.CreateBuilder(args);

// Configure serilog
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

// Registrar servicios de cada capa
builder.Services.AddWebServices();
builder.Services.AddBusinessServices();
builder.Services.AddCommonServices(builder.Configuration);
builder.Services.AddDataServices(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        // Ejecutar migraciones
        var migrator = services.GetRequiredService<IDatabaseMigrationService>();
        await migrator.MigrateDatabaseAsync();

        // Verificar conexión
        var canConnect = await migrator.CanConnectAsync();
        if (!canConnect)
        {
            app.Logger.LogError("No se puede conectar a la base de datos.");
            return;
        }

        // Ejecutar seeding solo si no hay datos iniciales
        var initializer = services.GetRequiredService<IDatabaseSetupService>();
        var hasInitialData = await initializer.HasInitialDataAsync();

        if (!hasInitialData)
        {
            await initializer.SeedAsync();
        }
        else
        {
            app.Logger.LogInformation("La base de datos ya contiene datos iniciales.");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error durante la inicialización de la aplicación.");
        throw;
    }
}

app.UseSession();
// Configure localization
app.UseRequestLocalization(localizationOptions);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
