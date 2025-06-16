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

        private const string TempDataErrorKey = "Error";
        private const string TempDataSuccessKey = "Satisfactorio";

        private const string UsuarioNoEncontradoMsg = "Usuario no encontrado o no pertenece al rol esperado.";
        private const string UsuarioNoEliminadoMsg = "Usuario no encontrado o no se pudo eliminar.";
        private const string UsuarioEliminadoMsg = "Usuario eliminado correctamente.";
        private const string UsuarioActualizadoMsg = "Usuario actualizado correctamente.";
        private const string ErrorAlEliminarMsg = "Error al eliminar el usuario.";

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
                TempData[TempDataErrorKey] = UsuarioNoEncontradoMsg;
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
                TempData[TempDataErrorKey] = UsuarioNoEncontradoMsg;
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
                    TempData[TempDataErrorKey] = UsuarioNoEncontradoMsg;
                    return RedirectToAction(nameof(Index));
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData[TempDataSuccessKey] = UsuarioActualizadoMsg;
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
                TempData[TempDataErrorKey] = UsuarioNoEncontradoMsg;
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
                TempData[TempDataErrorKey] = UsuarioNoEliminadoMsg;
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData[TempDataSuccessKey] = UsuarioEliminadoMsg;
                return RedirectToAction(nameof(Index));
            }

            TempData[TempDataErrorKey] = ErrorAlEliminarMsg;
            return RedirectToAction(nameof(Index));
        }
    }
}
