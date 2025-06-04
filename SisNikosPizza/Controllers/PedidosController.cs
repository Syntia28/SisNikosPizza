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

        public PedidosController(IUniwork unitWork, UserManager<IdentityUser> userManager)
        {
            _unitWork = unitWork;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var userSign = await _userManager.GetUserAsync(User);

            var pedidosDelivery = await _unitWork.DetallesPedidoRepo.ObtenerTodosAsync(
                // filtro: p => p.Pedido.TipoPedido == VCG.TipoPedido.Delivery,
                ordenarPor: p => p.OrderByDescending(p => p.Pedido.FechaPedido),
                incluirPropiedades: "Pedido,Pedido.User,Producto"
            );

            var pedidosAgrupados = pedidosDelivery
                .GroupBy(p => p.PedidoId)
                .Select(grupo => new VMDDetallesPedido
                {
                    PedidoId = grupo.Key,
                    FechaPedido = grupo.First().Pedido.FechaPedido,
                    Usuario = grupo.First().Pedido.User,
                    Detalles = grupo.ToList()
                }).ToList();

            return View(pedidosAgrupados);
        }

        // GET: para la vista del delivery
        public async Task<ActionResult> Delivery()
        {
            var pedidosDelivery = await _unitWork.DetallesPedidoRepo.ObtenerTodosAsync(
                filtro: p => p.Pedido.TipoPedido == VCG.TipoPedido.Delivery,
                ordenarPor: p => p.OrderByDescending(p => p.Pedido.FechaPedido),
                incluirPropiedades: "Pedido,Pedido.User,Producto"
            );

            var pedidosAgrupados = pedidosDelivery
                .GroupBy(p => p.PedidoId)
                .Select(grupo => new VMDDetallesPedido
                {
                    PedidoId = grupo.Key,
                    FechaPedido = grupo.First().Pedido.FechaPedido,
                    Usuario = grupo.First().Pedido.User,
                    Detalles = grupo.ToList()
                }).ToList();

            return View(pedidosAgrupados);
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
            //get role 
            // if (User.IsInRole(VCG.Role_Admin))
            // {


            //     return View(pedido);

            // }

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
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Content("error");
            }
        }

        [Authorize(Roles = VCG.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult> Cobrar(int id)
        {
            return View();
        }
    }
}
