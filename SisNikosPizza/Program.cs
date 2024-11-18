using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Implements;
using SisNikosPizza.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var conexion = builder.Configuration.GetConnectionString("ConnectionSQLServer");
builder.Services.AddDbContext<SisNikosPizzaBbContext>(options => options.UseSqlServer(conexion));
// Add services to the container.
builder.Services.AddControllersWithViews();

/// Agregar las refercnias de unitwork
builder.Services.AddScoped<IUniwork, UnitWork>();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
