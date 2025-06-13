using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Domain.ViewModels;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza.Controllers
{
    public class CarritoController : Controller

    {
        private readonly IUniwork _unitWork;
        private readonly UserManager<IdentityUser> _userManager;
        public CarritoController(IUniwork unitWork, UserManager<IdentityUser> userManager)
        {
            _unitWork = unitWork;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // var carritoItems = await _unitWork.CarritoItemsRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.ProductoId), incluirPropiedades: "Producto");
            // return View(carritoItems);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var carritoItems = _unitWork.CarritoItemsRepo.ListarCarritoItems(user?.Id).ToList();

            VMDCarrito VMDC = new VMDCarrito()
            {
                listaCarritoItems = carritoItems,
                Pedido = new Pedido(),
                Total = carritoItems.Sum(item => item.PrecioTotal)
            };

            return View(VMDC);
        }

        [HttpGet]
        public async Task<IActionResult> Total()
        {
            var total = await _unitWork.CarritoItemsRepo.ObtenerTodosAsync();
            int totalItems = total.Count();
            return Json(totalItems);
        }

        // crear un registro en el carrito
        [HttpPost]
        public async Task<IActionResult> Create(CarritoItems carritoItems)
        {
            if (!ModelState.IsValid)
                return Content("error:Datos no válidos");

            // 1. Obtener el producto desde la BD
            var producto = await _unitWork.ProductoRepo.ObtenerAsync(carritoItems.ProductoId);
            if (producto == null)
                return Content("error:Producto no encontrado");

            // 2. Verificar stock suficiente
            if (producto.Stock < carritoItems.Cantidad)
                return Content("error:Stock insuficiente");

            // 3. Descontar stock
            producto.Stock -= carritoItems.Cantidad;
            _unitWork.ProductoRepo.Actualizar(producto);

            // 4. Calcular precio total
            carritoItems.PrecioTotal = carritoItems.PrecioUnitario * carritoItems.Cantidad;

            // 5. Guardar el item en el carrito
            await _unitWork.CarritoItemsRepo.AgregarAsync(carritoItems);
            await _unitWork.GuardarAsync();

            return Content("ok");
        }


        [HttpPost]
        public async Task<IActionResult> Update(int CarritoItemId, int cantidad)
        {
            var carrito = await _unitWork.CarritoItemsRepo.ObtenerAsync(CarritoItemId);
            if (carrito is null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (cantidad > 0)
                {
                    carrito.Cantidad += cantidad;
                }
                else
                {
                    cantidad = Math.Abs(cantidad);
                    carrito.Cantidad -= cantidad;
                }
                carrito.PrecioTotal = carrito.PrecioUnitario * carrito.Cantidad;
                _unitWork.CarritoItemsRepo.ActualizarCantidadCarritoItems(carrito);
                await _unitWork.GuardarAsync();
                return Json(new { success = true, cantidad = carrito.Cantidad, total = carrito.PrecioTotal });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int CarritoItemId)
        {
            var carrito = await _unitWork.CarritoItemsRepo.ObtenerAsync(CarritoItemId);
            if (carrito is null)
            {   
                return NotFound();
            }

            _unitWork.CarritoItemsRepo.Eliminar(carrito);
            await _unitWork.GuardarAsync();
            return RedirectToAction("Index");
        }
    }
}
