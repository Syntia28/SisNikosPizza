using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Domain.ViewModels;

using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza.Controllers
{
    public class PedidosController : Controller
    {
        private readonly IUniwork _unitWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly gmail _correoUtilidad;


        public PedidosController(IUniwork unitWork, UserManager<IdentityUser> userManager, gmail correoUtilidad)
        {
            _unitWork = unitWork;
            _userManager = userManager;
            _correoUtilidad = correoUtilidad;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var pedidosDelivery = await _unitWork.DetallesPedidoRepo.GetPedidosAsync();
            return View(pedidosDelivery);
        }

        // GET: para la vista del delivery
        public async Task<ActionResult> Delivery()
        {
            var pedidosDelivery = await _unitWork.DetallesPedidoRepo.GetDeliveryPedidosAsync();
            return View(pedidosDelivery);
        }

        public async Task<ActionResult> Vaucher(int id)
        {
            var pedidosDelivery = await _unitWork.DetallesPedidoRepo.GetDetallesByPedidoIdAsync(id);
            return View(pedidosDelivery);
        }

        // GET: PedidodsController/Details/5
        public async Task<ActionResult> Detalles(int id)
        {
            var userSign = await _userManager.GetUserAsync(User);

            var pedido = await _unitWork.DetallesPedidoRepo.ObtenerPrimeroAsync(
                filtro: p => p.DetallePedidoId == id
            );
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CrearPedido(VMDCarrito VMD)
        {
            var userSign = await _userManager.GetUserAsync(User);

            if (userSign == null)
            {
                return RedirectToAction("Login", "Account");
            }

            VMD.Pedido.UserId = userSign.Id;
            VMD.Pedido.EstadoPedido = VCG.EstadoPedido.Pendiente;
            VMD.Pedido.FechaPedido = DateTime.Now;
            VMD.Pedido.Pagado = false;

            ModelState.Remove("Pedido.UserId");
            ModelState.Remove("Pedido.EstadoPedido");
            for (int i = 0; i < VMD.listaCarritoItems.Count; i++)
            {
                ModelState.Remove($"listaCarritoItems[{i}].UsuarioId");
            }

            if (ModelState.IsValid)
            {
                await _unitWork.PedidoRepo.AgregarAsync(VMD.Pedido);
                await _unitWork.GuardarAsync();

                if (VMD.listaCarritoItems == null || !VMD.listaCarritoItems.Any())
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var item in VMD.listaCarritoItems)
                {
                    var DetallePedido = new DetallePedido()
                    {
                        PedidoId = VMD.Pedido.PedidoId,
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        PrecioTotal = item.PrecioTotal
                    };
                    await _unitWork.DetallesPedidoRepo.AgregarAsync(DetallePedido);
                    var carritoItems = _unitWork.CarritoItemsRepo.ListarCarritoItems(userSign.Id).ToList();
                    foreach (var carritoItem in carritoItems)
                    {
                        _unitWork.CarritoItemsRepo.Eliminar(carritoItem);
                    }

                    await _unitWork.GuardarAsync();

                    _correoUtilidad.CorreoDeCompra(userSign.Email, userSign.UserName);

                }
                return RedirectToAction(nameof(Vaucher), new { id = VMD.Pedido.PedidoId });
            }
            else
            {
                return Content("error");
            }
        }

        [Authorize(Roles = VCG.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult> Cobrar(int id, string userid)
        {
            var userlogin = await _userManager.GetUserAsync(User);
            var pedido = await _unitWork.PedidoRepo.CobrarPedido(
                pedidoId: id,
                UserId: userid
            );

            if (await _userManager.IsInRoleAsync(userlogin, VCG.Role_Admin))
            {
                return RedirectToAction(nameof(Index));
            }
            else if (await _userManager.IsInRoleAsync(userlogin, VCG.Role_Delivery))
            {
                return RedirectToAction(nameof(Delivery));
            }
            return View(nameof(Index));
        }
    }
}
