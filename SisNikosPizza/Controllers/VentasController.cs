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

            var pedidosAgrupados = await _unitWork.DetallesPedidoRepo.GetVentasAsync();

            return View(pedidosAgrupados);
        }

        [HttpGet]
        public async Task<ActionResult> Detalles(int id)
        {
            var pedidosAgrupados = await _unitWork.DetallesPedidoRepo.GetDetallesByPedidoIdAsync(id);
            return View(pedidosAgrupados);
        }
    }
}
