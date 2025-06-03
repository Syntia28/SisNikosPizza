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
        var detallePedidos = (await _unitWork.DetallesPedidoRepo.ObtenerTodosAsync()).ToList();

        byte[] archivoExcel = Reportes.FromList(detallePedidos, "DetallesPedido");

        return File(
            fileContents: archivoExcel,
            contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileDownloadName: "ReporteDetallesPedido.xlsx"
        );
    }
}
