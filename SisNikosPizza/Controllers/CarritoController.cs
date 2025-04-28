
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;

namespace SisNikosPizza.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly IUniwork _unitWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CarritoController(IUniwork unitWork, UserManager<IdentityUser> userManager)
        {
            _unitWork = unitWork;
            _userManager = userManager;
        }

        // GET: /Carrito/
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            var carrito = await _unitWork.CarritoRepo.ObtenerCarritoDeUsuario(userId, baseUrl);

            if (carrito == null)
            {
                carrito = new Carrito
                {
                    CarritoProductos = new List<CarritoProducto>()
                };
            }

            return View(carrito);
        }

        // POST: /Carrito/AgregarProducto
        [HttpPost]
        public async Task<IActionResult> AgregarProducto(int productoId)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            await _unitWork.CarritoRepo.AgregarProductoAlCarrito(userId, productoId, 1);

            return RedirectToAction("Index");
        }

        // POST: /Carrito/IncrementarCantidad
        [HttpPost]
        public async Task<IActionResult> IncrementarCantidad(int productoId)
        {
            var userId = _userManager.GetUserId(User);

            await _unitWork.CarritoRepo.IncrementarCantidadProducto(userId, productoId);

            return RedirectToAction("Index");
        }

        // POST: /Carrito/DecrementarCantidad
        [HttpPost]
        public async Task<IActionResult> DecrementarCantidad(int productoId)
        {
            var userId = _userManager.GetUserId(User);

            await _unitWork.CarritoRepo.DecrementarCantidadProducto(userId, productoId);

            return RedirectToAction("Index");
        }

        // POST: /Carrito/EliminarProducto
        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int productoId)
        {
            var userId = _userManager.GetUserId(User);

            await _unitWork.CarritoRepo.EliminarProductoDeCarrito(userId, productoId);

            return RedirectToAction("Index");
        }

        // GET: /Carrito/ObtenerTotal
        [HttpGet]
        public async Task<IActionResult> ObtenerTotal()
        {
            var userId = _userManager.GetUserId(User);

            var total = await _unitWork.CarritoRepo.ObtenerTotalCarrito(userId);

            return Json(total);
        }
    }
}
