using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Utilidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisNikosPizza.Controllers
{
    [Authorize(Roles= VCG.Role_Admin)]
    public class ClienteController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClienteController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var usersInRole = new List<IdentityUser>();

            // Filtrar usuarios que tienen el rol "UserClient"
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, VCG.Role_Usuario))
                {
                    usersInRole.Add(user);
                }
            }

            // Pasar los usuarios filtrados a la vista
            return View(usersInRole);
        }
    }
}
