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


        [Authorize(Roles = VCG.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult> CrearVenta(int id)
        {

            Console.WriteLine("id recivido es: " + id);
            //obtener el pedido.
            var pedido = await _unitWork.PedidoRepo.ObtenerPedidoDetallado(id, "");

            Console.WriteLine("pedido recivido es: " + pedido);

            //verificar que exista el pedido
            if (pedido==null)
            {
                Console.WriteLine("pedido es null");
                return NotFound();
            }

            //veririficar que el pedido no este pagado
            if (pedido.EstadoPedido == VCG.EstadoPedido.Pagado)
            {
                Console.WriteLine("pedido es pagado");
                return NotFound();
            }




            //verificar que existan productos en el pedido.

            Console.WriteLine("pedido tiene detalles: " + pedido.DetallePedidos.Count);

            if (pedido.DetallePedidos.Count <=0)
            {
                Console.WriteLine("pedido esta vacio");
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
            await _unitWork.GuardarVenta();

         
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
