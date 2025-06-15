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
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> DescargarExcel()
    {
        var pedidosDelivery = await _unitWork.DetallesPedidoRepo.ObtenerTodosAsync(
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
               Total = g.Sum(r => r.PrecioTotal)
           }).ToList();
        string nombreArchivo = $"Reporte-Ventas-{DateTime.Now:yyyy-mm-dd}";

        byte[] archivoExcel = ReportesExel.FromList(Reporte, nombreArchivo);

        return File(
            fileContents: archivoExcel,
            contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileDownloadName: $"{nombreArchivo}.xlsx"
        );
    }

    [HttpGet]
    public async Task<IActionResult> DescargarPdf()
    {
        var pedidosDelivery = await _unitWork.DetallesPedidoRepo.ObtenerTodosAsync(
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
               Total = g.Sum(r => r.PrecioTotal)
           }).ToList();
        string nombreArchivo = $"Reporte-Ventas-{DateTime.Now:yyyy-mm-dd}";

        byte[] archivoPdf = ReportesPdf.FromList(Reporte, nombreArchivo);

        return File(
            fileContents: archivoPdf,
            contentType: "application/pdf",
            fileDownloadName: $"{nombreArchivo}.pdf"
        );
    }

    [HttpPost]
    public async Task<IActionResult> ExportarBoleta(int id)
    {
        Console.WriteLine($"id recibido en la solicitud: {id}");
        var detalle = await _unitWork.DetallesPedidoRepo.GetDetallesByPedidoIdAsync(id);

        var detallesUsuario = new List<VMDDetallesUsuaio>
        {
            new VMDDetallesUsuaio { Columna = "Cliente", Contenido = detalle.Usuario.UserName }
        };
        var direccion = detalle.Detalles.FirstOrDefault()?.Pedido?.Direccion;
        if (!string.IsNullOrWhiteSpace(direccion))
        {
            detallesUsuario.Add(new VMDDetallesUsuaio { Columna = "Dirección", Contenido = direccion });
        }

        var boleta = new VMDBoleta
        {
            id = detalle.PedidoId,
            Fecha = detalle.Detalles.FirstOrDefault()?.Pedido?.FechaPedido ?? DateTime.Now,

            DetallesPedido = detalle.Detalles.Select(dp => new VMDDPedidoBoleta
            {
                cantidad = dp.Cantidad,
                producto = dp.Producto?.Nombre ?? "Producto desconocido",
                Total = dp.PrecioTotal
            }),

            DetallesUsuario = detallesUsuario,

            Total = (double)detalle.Detalles.Sum(d => d.PrecioTotal),

            MetodoEntrega = detalle.Detalles.First().Pedido.TipoPedido.ToString()
        };

        byte[] archivoPdf = ReportesPdf.GenerarBoletaPDF(boleta);
        return File(
            fileContents: archivoPdf,
            contentType: "application/pdf",
            fileDownloadName: $"Boleta-{id}-{DateTime.Now:yyyy-MM-dd}.pdf"
        );
    }

    public async Task<IActionResult> Export()
    {
        var boleta = new VMDBoleta
        {
            id = 1,
            Fecha = DateTime.Now,
            DetallesPedido = new List<VMDDPedidoBoleta>
            {
                new VMDDPedidoBoleta { cantidad = 4, producto = "producto 1", Total = (decimal)45.0 },
                new VMDDPedidoBoleta { cantidad = 5, producto = "producto 2", Total = (decimal)145.0 },
                new VMDDPedidoBoleta { cantidad = 6, producto = "producto 3", Total = (decimal)245.0 }
            },
            DetallesUsuario = new List<VMDDetallesUsuaio>
            {
                new VMDDetallesUsuaio { Columna = "Cliente", Contenido = "Ruben" },
                new VMDDetallesUsuaio { Columna = "Direccion", Contenido = "av.systems" }
            },
            Total = 435.0,
            MetodoEntrega = "Delivery"
        };


        byte[] archivoPdf = ReportesPdf.GenerarBoletaPDF(boleta);

        return File(
            fileContents: archivoPdf,
            contentType: "application/pdf",
            fileDownloadName: $"Boleta-{DateTime.Now:yyyy-MM-dd}.pdf"
        );
    }
}
