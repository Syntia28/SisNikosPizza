using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;
using System.Data;

namespace SisNikosPizza.Controllers
{
    public class VentaController : Controller
    {
        // GET: VentaController
        private readonly IUniwork _unitWork;
        private readonly UserManager<IdentityUser> _userManager;

        public VentaController(
            IUniwork uniwork,
            UserManager<IdentityUser> userManager
            )
        {
            _unitWork = uniwork;
            _userManager = userManager;
        }

        [Authorize(Roles = VCG.Role_Admin)]
        public async Task<ActionResult> Index()
        {
            
            var ventas =await _unitWork.VentaRepo.ObtenerVentasDetallados();
    
            return View(ventas);
        }

        // GET: VentaController/Details/5
        public async Task<ActionResult> Detalles(int id)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            var venta = await _unitWork.VentaRepo.ObtenerVentaDetallado(id, baseUrl);

            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var ventas = await _unitWork.VentaRepo.ObtenerVentasDetallados();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ventas");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "FECHA";
                worksheet.Cell(1, 3).Value = "CLIENTE";
                worksheet.Cell(1, 4).Value = "DETALLE";
                worksheet.Cell(1, 5).Value = "TOTAL";

                int row = 2;
                foreach (var v in ventas)
                {
                    worksheet.Cell(row, 1).Value = v.VentaId;
                    worksheet.Cell(row, 2).Value = v.Fecha;
                    worksheet.Cell(row, 3).Value = v.Owner.Email;

                    string detalles = "";
                    foreach (var det in v.Detalles)
                    {
                        detalles += det.Producto?.Nombre + "\n";
                    }

                    worksheet.Cell(row, 4).Value = detalles;
                    worksheet.Cell(row, 5).Value = v.Total;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "ReporteVentas.xlsx");
                }
            }
        }


        [Authorize(Roles = VCG.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult> CrearVenta(int id)
        {

            //obtener el pedido.
            var pedido = await _unitWork.PedidoRepo.ObtenerPedidoDetallado(id, "");

            //verificar que exista el pedido
            if (pedido==null)
            {
                return NotFound();
            }

            //veririficar que el pedido no este pagado
            if (pedido.EstadoPedido == VCG.EstadoPedido.Pagado)
            {
                return NotFound();
            }




            //verificar que existan productos en el pedido.


            if (pedido.DetallePedidos.Count <=0)
            {
                return NotFound();//mas a futuro dejar que si el pedido esta vacio, entonces puedes agregar producto al cobrar le venta
            }
            //obtener propiedadesss de la venta
            var venta = new Venta();
            var detallesVentas = new List<DetalleVenta>();

            foreach (var pedidoProducto in pedido.DetallePedidos)
            {
                var detalleVenta = new DetalleVenta()
                {
                    ProductoId = pedidoProducto.ProductoId,
                    PrecioTotal = pedidoProducto.Cantidad * pedidoProducto.Producto.Precio,
                    PrecioUnitario = pedidoProducto.Producto.Precio,
                    Cantidad = pedidoProducto.Cantidad,
                    VentaId = venta.VentaId,
                    

                };
                detallesVentas.Add(detalleVenta);
            }
            
            venta.Detalles = detallesVentas;
            venta.Fecha = DateTime.Now;
            venta.OwnerId = pedido.OwnerId;

            //actualizar el pedido
            pedido.EstadoPedido = VCG.EstadoPedido.Pagado;
          



            // guardar venta
            await _unitWork.VentaRepo.AgregarAsync(venta);
            await _unitWork.GuradarAsync();

         
            // redireccionar a index
            return RedirectToAction(nameof(Index));

        }

        // POST: VentaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VentaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VentaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VentaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VentaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
