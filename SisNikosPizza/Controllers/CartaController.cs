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
            // Instanciamos la clase gmail para usar sus métodos
            gmail correoUtilidad = new gmail();

            // Suponiendo que tienes un usuario que ha pedido la recuperación de su cuenta
            string correoDestino = "rubencithochavez036@gmail.com"; // Correo del usuario 
            string nombreUsuario = "rdev system"; // Nombre del usuario para personalizar el mensaje

            // Llamamos al método de enviar correo con el mensaje generado
            correoUtilidad.CorreoDeCompra(correoDestino, nombreUsuario);

            // Retornar una vista o mensaje de confirmación
            return Content("Correo enviado correctamente.");

            //            return View();
        }

    }
}
