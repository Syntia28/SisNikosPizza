using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Implements;
using SisNikosPizza.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var conexion = builder.Configuration.GetConnectionString("ConnectionSQLServer");
builder.Services.AddDbContext<SisNikosPizzaBbContext>(options => options.UseSqlServer(conexion));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SisNikosPizzaBbContext>();
// Add services to the container.
builder.Services.AddControllersWithViews();

/// Agregar las refercnias de unitwork
builder.Services.AddScoped<IUniwork, UnitWork>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();

//builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();



// Configurar una ruta espec�fica para las im�genes
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "images")),
    RequestPath = "/images"
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
