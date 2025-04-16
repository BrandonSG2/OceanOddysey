using Microsoft.EntityFrameworkCore;
using OceanOdyssey.Infraestructure.Data;
using Serilog.Events;
using Serilog;
using System.Text;
using OceanOdyssey.Web.Middleware;
using OceanOdyssey.Infraestructure.Repository.Interfaces;
using OceanOdyssey.Application.Services.Interfaces;
using OceanOdyssey.Application.Profiles;
using OceanOdyssey.Infraestructure.Repository.Implementations;
using OceanOdyssey.Application.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OceanOdyssey.Application.Config;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);
var cultureInfo = new CultureInfo("es-CR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
// Mapeo de la clase AppConfig para leer appsettings.json
builder.Services.Configure<AppConfig>(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configurar D.I.
//Repository
builder.Services.AddTransient<IRepositoryBarco, RepositoryBarco>();
builder.Services.AddTransient<IRepositoryHabitacion, RepositoryHabitacion>();
builder.Services.AddTransient<IRepositoryResumenReservacion, RepositoryResumenReservacion>();
builder.Services.AddTransient<IRepositoryCrucero, RepositoryCrucero>();
builder.Services.AddTransient<IRepositoryPuerto, RepositoryPuerto>();
builder.Services.AddTransient<IRepositoryBarcoHabitacion, RepositoryBarcoHabitacion>();
builder.Services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddTransient<IRepositoryPais, RepositoryPais>();
builder.Services.AddTransient<IRepositoryFechaCrucero, RepositoryFechaCrucero>();
builder.Services.AddTransient<IRepositoryComplemento, RepositoryComplemento>();
builder.Services.AddTransient<IRepositoryPasajero, RepositoryPasajero>();
builder.Services.AddTransient<IRepositoryPDF, RepositoryPDF>();
//Services
builder.Services.AddTransient<IServiceBarco, ServiceBarco>();
builder.Services.AddTransient<IServiceHabitacion, ServiceHabitacion>();
builder.Services.AddTransient<IServiceResumenReservacion, ServiceResumenReservacion>();
builder.Services.AddTransient<IServiceCrucero, ServiceCrucero>();
builder.Services.AddTransient<IServicePuerto , ServicePuerto>();
builder.Services.AddTransient<IServiceBarcoHabitacion, ServiceBarcoHabitacion>();
builder.Services.AddTransient<IServiceUsuario, ServiceUsuario>();
builder.Services.AddTransient<IServicePais, ServicePais>();
builder.Services.AddTransient<IServiceFechaCrucero, ServiceFechaCrucero>();
builder.Services.AddTransient<IServiceComplemento, ServiceComplemento>();
builder.Services.AddTransient<IServicePasajero, ServicePasajero>();
builder.Services.AddTransient<IServiceCambio, ServiceCambio>();
//Seguridad
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.AccessDeniedPath = "/Login/Forbidden/";
    });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None,
            }
        );
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
    });
//Configurar Automapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<BarcoProfile>();
    config.AddProfile<HabitacionProfile>();
    config.AddProfile<ResumenReservacionProfile>();
    config.AddProfile<BarcoHabitacionProfile>();
    config.AddProfile<FechaCruceroProfile>();
    config.AddProfile<ReservaComplementoProfile>();
    config.AddProfile<PuertoProfile>();
    config.AddProfile<PaisProfile>();
    config.AddProfile<ReservaHabitacionProfile>();
    config.AddProfile<ItinerarioProfile>();
    config.AddProfile<ComplementoProfile>();
    config.AddProfile<CruceroProfile>();
    config.AddProfile<PasajeroProfile>();
    config.AddProfile<UsuarioProfile>();
    config.AddProfile<PrecioHabitacionProfile>();
});
// Configuar Conexión a la Base de Datos SQL
builder.Services.AddDbContext<OceanOdysseyContext>(options =>
{
    // it read appsettings.json file
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDataBase"));
    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});
//***********************
//Configuración Serilog
// Logger. P.E. Verbose = muestra SQl Statement
var logger = new LoggerConfiguration()
                    // Limitar la información de depuración
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                    .Enrich.FromLogContext()
                    // Log LogEventLevel.Verbose muestra mucha información, pero no es necesaria solo para el proceso de depuración
                    .WriteTo.Console(LogEventLevel.Information)
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", shared: true, encoding: System.Text.Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", shared: true, encoding: Encoding.ASCII, rollingInterval: RollingInterval.Day))
                    .CreateLogger();

builder.Host.UseSerilog(logger);
//***************************


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Error control Middleware
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

var supportedCultures = new[] { cultureInfo };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

//Activar soporte a la solicitud de registro con SERILOG
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Activar Antiforgery
app.UseAntiforgery();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
