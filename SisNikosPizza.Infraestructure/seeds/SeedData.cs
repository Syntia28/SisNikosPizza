using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Utilidades; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace F_M_Maquinarias.Infrastructure.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new SisNikosPizzaBbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<SisNikosPizzaBbContext>>()))
        {
            // Verificar si hay migraciones pendientes y aplicarlas
            context.Database.Migrate();
        }


        
    }
}