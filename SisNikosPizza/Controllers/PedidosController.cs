using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            //get role 
          if (User.IsInRole(VCG.Role_Admin)) {
                var pedidos = await _unitWork.PedidoRepo.ObtenerPedidosDetalladosTodos();
                return View(pedidos);
            }

            var pedidosUsuario = await _unitWork.PedidoRepo.ObtenerPedidosDetallados(userSign.Id);

            return View(pedidosUsuario);
        }

        // GET: para la vista del delivery
        public async Task<ActionResult> Delivery()
        {
            var pedidosDelivery = await _unitWork.DetallesPedidoRepo.ObtenerTodosAsync(
                filtro: p => p.Pedido.TipoPedido == VCG.TipoPedido.Delivery,
                ordenarPor: p => p.OrderByDescending(p => p.Pedido.FechaPedido),
                incluirPropiedades: "Pedido,Pedido.Owner,Producto"
            );


            return View(pedidosDelivery);
        }

        // GET: PedidodsController/Details/5
        public async Task<ActionResult> Detalles(int id)
        {
            var userSign = await _userManager.GetUserAsync(User);
            var baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            var pedido = await _unitWork.PedidoRepo.ObtenerPedidoDetallado(id, baseUrl);
            if (pedido == null)
            {
                return NotFound();
            }
            //get role 
            if (User.IsInRole(VCG.Role_Admin))
            {


                return View(pedido);

            }


            if (pedido.UserId != userSign.Id)
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
