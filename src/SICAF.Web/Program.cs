using System.Globalization;

using DotNetEnv;

using Microsoft.AspNetCore.Localization;

using Serilog;

using SICAF.Business;
using SICAF.Common;
using SICAF.Data;
using SICAF.Data.Interfaces;
using SICAF.Web;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Env.Load(); // Load .env
}

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
if (app.Environment.IsDevelopment())
{
    await InitializeDatabaseAsync(app);
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
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

static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var migrator = services.GetRequiredService<IDatabaseMigrationService>();
        var canConnect = await migrator.CanConnectAsync();
        if (!canConnect)
        {
            app.Logger.LogError("No se puede conectar a la base de datos.");
            return;
        }
        // execute database migrations
        await migrator.MigrateDatabaseAsync();
        // execute database setup
        var initializer = services.GetRequiredService<IDatabaseSetupService>();
        var hasInitialData = await initializer.HasInitialDataAsync();
        if (!hasInitialData) await initializer.SeedAsync();
        else app.Logger.LogInformation("La base de datos ya contiene datos iniciales.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error durante la inicialización de la aplicación.");
        throw;
    }
}