using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Implements;
using SisNikosPizza.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using F_M_Maquinarias.Infrastructure.Data;
using SisNikosPizza.Utilidades;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
});

// Agregar la cadena de conexión a utilizar
var conexion = builder.Configuration.GetConnectionString("ConnectionSQLServer");
builder.Services.AddDbContext<SisNikosPizzaBbContext>(options => options.UseSqlServer(conexion));

// Configurar Identity con opciones mejoradas
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Configuración de cuenta
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;

    // Configuración de contraseña
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Configuración de bloqueo
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Configuración de usuario
    options.User.RequireUniqueEmail = true;
})
.AddDefaultUI()
.AddEntityFrameworkStores<SisNikosPizzaBbContext>()
.AddDefaultTokenProviders();

// Configurar cookies de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Agregar las referencias a la unidad de trabajo (UnidadDeTrabajo)
builder.Services.AddScoped<IUniwork, UnitWork>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<gmail>();

// Datos iniciales
builder.Services.AddScoped<IDbInitialize, DbInitialize>();

var app = builder.Build();

// Inicializar datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
    try
    {
        var inicializador = services.GetRequiredService<IDbInitialize>();
        inicializador.Initialize();
    }
    catch (Exception ex)
    {
        // Log the exception properly
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error durante la inicialización de la base de datos.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Create a new FileExtensionContentTypeProvider and add the AVIF mapping
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".avif"] = "image/avif";

// Configure static files to use a long cache lifetime and the custom content type provider
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    OnPrepareResponse = ctx =>
    {
        const int durationInSeconds = 60 * 60 * 24 * 365; // 1 year
        ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={durationInSeconds}");
    }
});

app.UseRouting();

// ⚠️ IMPORTANTE: Agregar UseAuthentication() ANTES de UseAuthorization()
app.UseAuthentication(); // 👈 ESTO FALTABA EN TU CÓDIGO
app.UseAuthorization();

// Use response compression
app.UseResponseCompression();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();