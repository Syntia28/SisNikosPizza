using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Utilidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisNikosPizza.Controllers
{
    [Authorize(Roles = VCG.Role_Admin)]
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

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Domain.Models.ApplicationUser us)
        {
            await _userManager.UpdateAsync(us);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Domain.Models.ApplicationUser us)
        {
            if (ModelState.IsValid)
            {
                await _userManager.UpdateAsync(us);

                RedirectToAction("Index");
            }

            return View(us);
        }
    }
}
