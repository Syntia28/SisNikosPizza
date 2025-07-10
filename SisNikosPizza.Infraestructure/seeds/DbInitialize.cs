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
            Email = "adminNikos@sys.ss",
            UserName = "admin@sys.ss",
            PhoneNumber = "123456789",
        };

        var employe = new ApplicationUser
        {
            Email = "employe@sys.ss",
            UserName = "employe@sys.ss",
            PhoneNumber = "123456789",
        };

        var user = new ApplicationUser
        {
            Email = "user@sys.ss",
            UserName = "user@sys.ss",
            PhoneNumber = "123456789",
        };

        var manager = new ApplicationUser
        {
            Email = "manager@sys.ss",
            UserName = "manager@sys.ss",
            PhoneNumber = "123456789",
        };

        var delivery = new ApplicationUser
        {
            Email = "delivery@dev.ss",
            UserName = "delivery@dev.ss",
            PhoneNumber = "123456789",
        };

        _userManager.CreateAsync(admin, "Admin123").GetAwaiter().GetResult();
        _userManager.CreateAsync(user, "Usuario123").GetAwaiter().GetResult();
        _userManager.CreateAsync(employe, "Empleado123").GetAwaiter().GetResult();
        _userManager.CreateAsync(manager, "Manager123").GetAwaiter().GetResult();
        _userManager.CreateAsync(delivery, "Delivery123").GetAwaiter().GetResult();

        _userManager.AddToRoleAsync(admin, VCG.Role_Admin).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(user, VCG.Role_Usuario).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(employe, VCG.Role_Employee).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(manager, VCG.Role_Manager).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(delivery, VCG.Role_Delivery).GetAwaiter().GetResult();
    }
}
