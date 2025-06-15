using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain;
using SisNikosPizza.Domain.ViewModels;
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

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, VCG.Role_Usuario))
                {
                    usersInRole.Add(user);
                }
            }

            return View(usersInRole);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, VCG.Role_Usuario))
            {
                TempData["Error"] = "Usuario no encontrado o no pertenece al rol esperado.";
                return RedirectToAction(nameof(Index));
            }

            return View(user); 
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, VCG.Role_Usuario))
            {
                TempData["Error"] = "Usuario no encontrado o no pertenece al rol esperado.";
                return RedirectToAction(nameof(Index));
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null || !await _userManager.IsInRoleAsync(user, VCG.Role_Usuario))
                {
                    TempData["Error"] = "Usuario no encontrado o no pertenece al rol esperado.";
                    return RedirectToAction(nameof(Index));
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Satisfactorio"] = "Usuario actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model); 
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, VCG.Role_Usuario))
            {
                TempData["Error"] = "Usuario no encontrado o no pertenece al rol esperado.";
                return RedirectToAction(nameof(Index));
            }

            return View(user); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, VCG.Role_Usuario))
            {
                TempData["Error"] = "Usuario no encontrado o no se pudo eliminar.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Satisfactorio"] = "Usuario eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al eliminar el usuario.";
            return RedirectToAction(nameof(Index));
        }
    }
}
