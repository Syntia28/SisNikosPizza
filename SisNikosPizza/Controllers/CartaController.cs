using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza.Controllers
{
    public class CartaController : Controller

    {
        private readonly IUniwork _unitWork;
        private readonly UserManager<IdentityUser> _userManager;
        public CartaController(IUniwork unitWork, UserManager<IdentityUser> userManager)
        {
            _unitWork = unitWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productos = await _unitWork.ProductoRepo.ObtenerTodosAsync(
            ordenarPor: c => c.OrderByDescending(c => c.ProductoId));

            var user = await _userManager.GetUserAsync(User);
            ViewBag.UsuarioId = user?.Id;

            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            gmail correoUtilidad = new gmail();

            string correoDestino = "rubencithochavez036@gmail.com"; 
            string nombreUsuario = "rdev system"; 

            correoUtilidad.CorreoDeCompra(correoDestino, nombreUsuario);

            return Content("Correo enviado correctamente.");

        }

    }
}
