using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza
{
    public static class SeedData
    {
        private static async Task CreateRolesIfNotExists(RoleManager<IdentityRole> roleManager)
        {
            // Crear los roles si no existen
            // Registrar los roles
            if (!await roleManager.RoleExistsAsync(VCG.Role_Admin))
                await roleManager.CreateAsync(new IdentityRole(VCG.Role_Admin));
            if (!await roleManager.RoleExistsAsync(VCG.Role_Employee))
                await roleManager.CreateAsync(new IdentityRole(VCG.Role_Employee));
            if (!await roleManager.RoleExistsAsync(VCG.Role_Manager))
                await roleManager.CreateAsync(new IdentityRole(VCG.Role_Manager));
            if (!await roleManager.RoleExistsAsync(VCG.Role_Usuario))
                await roleManager.CreateAsync(new IdentityRole(VCG.Role_Usuario));
        }

        private static async Task CreateAdminUserIfNotExists(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            // Crear el usuario administrador si no existe
            var adminEmail = configuration.GetValue<string>("Admin:Email");
            var adminPassword = configuration.GetValue<string>("Admin:Password");

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                await userManager.CreateAsync(adminUser, adminPassword);
                await userManager.AddToRoleAsync(adminUser, VCG.Role_Admin);
            }
        }
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            using (var context = new SisNikosPizzaBbContext(
                serviceProvider.GetRequiredService<DbContextOptions<SisNikosPizzaBbContext>>()))
            {

                await CreateRolesIfNotExists(roleManager);

                // Crear el usuario administrador
                await CreateAdminUserIfNotExists(userManager, configuration);

            }
        }

         
    }
}
