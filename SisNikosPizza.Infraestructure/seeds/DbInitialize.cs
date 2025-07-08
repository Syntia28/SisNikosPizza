using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace F_M_Maquinarias.Infrastructure.Data;

public class DbInitialize : IDbInitialize
{
	private readonly SisNikosPizzaBbContext _context;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoleManager<IdentityRole> _userRole;
    public DbInitialize(SisNikosPizzaBbContext context, RoleManager<IdentityRole> userRole, UserManager<IdentityUser> userManager)
    {
        _context = context;
		_userManager = userManager;
        _userRole = userRole;
    }
    public void Initialize()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }
        }
        catch (Exception)
        {
            throw;
        }

        if (_context.Roles.Any()) return;

        _userRole.CreateAsync(new IdentityRole(VCG.Role_Admin)).GetAwaiter().GetResult();
        _userRole.CreateAsync(new IdentityRole(VCG.Role_Employee)).GetAwaiter().GetResult();
        _userRole.CreateAsync(new IdentityRole(VCG.Role_Usuario)).GetAwaiter().GetResult();
        _userRole.CreateAsync(new IdentityRole(VCG.Role_Manager)).GetAwaiter().GetResult();
        _userRole.CreateAsync(new IdentityRole(VCG.Role_Delivery)).GetAwaiter().GetResult();

        var admin = new ApplicationUser
        {
            Email = "admin@sys.cs",
            UserName = "admin@sys.cs",
            PhoneNumber = "123456789",
        };

        var employe = new ApplicationUser
        {
            Email = "employe@sys.cs",
            UserName = "employe@sys.cs",
            PhoneNumber = "123456789",
        };

        var user = new ApplicationUser
        {
            Email = "user@sys.cs",
            UserName = "user@sys.cs",
            PhoneNumber = "123456789",
        };

        var manager = new ApplicationUser
        {
            Email = "manager@sys.cs",
            UserName = "manager@sys.cs",
            PhoneNumber = "123456789",
        };

        var delivery = new ApplicationUser
        {
            Email = "delivery@sys.cs",
            UserName = "delivery@sys.cs",
            PhoneNumber = "123456789",
        };

        _userManager.CreateAsync(admin, "Admin123*").GetAwaiter().GetResult();
        _userManager.CreateAsync(user, "Usuario123*").GetAwaiter().GetResult();
        _userManager.CreateAsync(employe, "Empleado123*").GetAwaiter().GetResult();
        _userManager.CreateAsync(manager, "Manager123*").GetAwaiter().GetResult();
        _userManager.CreateAsync(delivery, "Delivery123*").GetAwaiter().GetResult();

        _userManager.AddToRoleAsync(admin, VCG.Role_Admin).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(user, VCG.Role_Usuario).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(employe, VCG.Role_Employee).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(manager, VCG.Role_Manager).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(delivery, VCG.Role_Delivery).GetAwaiter().GetResult();
    }
}
