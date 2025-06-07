using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Domain.ViewModels;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;
using System.Diagnostics;

namespace SisNikosPizza.Controllers;

public class ReportesController : Controller
{
    private readonly ILogger<ReportesController> _logger;
    private readonly IUniwork _unitWork;

    public ReportesController(IUniwork unitWork, ILogger<ReportesController> logger)
    {
        _unitWork = unitWork;
    }

    [HttpGet]
    public async Task<IActionResult> DescargarExcel()
    {
        var pedidosDelivery = await _unitWork.DetallesPedidoRepo.ObtenerTodosAsync(
                // filtro: p => p.Pedido.TipoPedido == VCG.TipoPedido.Delivery,
                ordenarPor: p => p.OrderByDescending(p => p.Pedido.FechaPedido),
                incluirPropiedades: "Pedido,Pedido.User,Producto"
            );
        var Reporte = pedidosDelivery
           .GroupBy(R => R.ProductoId)
           .Select(g => new VMDReportes
           {
               PedidoId = g.Key,
               FechaPedido = g.First().Pedido.FechaPedido,
               Cliente = g.First().Pedido.User.UserName,
               Productos = g.Count(),
               Total = g.Sum( r => r.PrecioTotal)
           }).ToList();
        string nombreArchivo = $"Reporte-Ventas-{DateTime.Now:yyyy-mm-dd}";

        byte[] archivoExcel = Reportes.FromList(Reporte, nombreArchivo);

        return File(
            fileContents: archivoExcel,
            contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileDownloadName: $"{nombreArchivo}.xlsx"
        );
    }
}
