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
    public class VentasController : Controller
    {
        private readonly IUniwork _unitWork;
        private readonly UserManager<IdentityUser> _userManager;

        public VentasController(IUniwork unitWork, UserManager<IdentityUser> userManager)
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
        [Authorize(Roles = VCG.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult> Cobrar(int id)
        {
            return View();
        }
    }
}
