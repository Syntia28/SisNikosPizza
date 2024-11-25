using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Implements;
using SisNikosPizza.Repository.Interfaces;

namespace SisNikosPizza.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IUniwork _unitWork;
        private readonly UserManager<IdentityUser> _userManager;
        public CarritoController (
          IUniwork unitWork, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            //_context = context;
            _unitWork = unitWork;

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            Carrito carrito = await _unitWork.CarritoRepo.ObtenerCarritoDeUsuario(_userManager.GetUserId(User),baseUrl);
            return View(carrito);
        }

        //post AgregarProducto
        [Authorize]
        [HttpPost]
public async Task<IActionResult> AgregarProducto (Producto producto)
        {
           
            //obtiene el carrito del usuario,
            await _unitWork.CarritoRepo.AgregarProductoAlCarrito(_userManager.GetUserId(User), producto.ProductoId, 1);

            //redirect to index
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ObtenerTotal()
        {

            int total = await _unitWork.CarritoRepo.ObtenerTotalCarrito(_userManager.GetUserId(User));   
            return Json(total);
        }
       
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EliminarProducto(Producto p)
        {
            await _unitWork.CarritoRepo.EliminarProductoDeCarrito(_userManager.GetUserId(User), p.ProductoId);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> IncrementarCantidad(Producto p)
        {
            await _unitWork.CarritoRepo.IncrementarCantidadProducto(_userManager.GetUserId(User), p.ProductoId);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DecrementarCantidad(Producto p)
        {
            await _unitWork.CarritoRepo.DecrementarCantidadProducto(_userManager.GetUserId(User), p.ProductoId);
            return RedirectToAction("Index");
        }
    }
}
