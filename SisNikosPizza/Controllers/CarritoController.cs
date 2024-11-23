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
    }
}
