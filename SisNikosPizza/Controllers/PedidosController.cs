using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Models;
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


        // GET: PedidodsController/Details/5
        public ActionResult Detalles(int id)
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Nuevo(string TipoPedido)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            var carrito = await _unitWork.CarritoRepo.ObtenerCarritoDeUsuario(_userManager.GetUserId(User),baseUrl);


           var carritoProductos = carrito.CarritoProductos;
            var nuevoPedidoVista = new NuevoPedidoVista()
            {
                CarritoProductos = carritoProductos.ToList(),
                Pedido = new Pedido()
            };
            ViewBag.TipoPedido = TipoPedido;
            return View(nuevoPedidoVista);
        }





        // POST: PedidodsController/Create
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CrearPedido(Pedido formPedido)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            var carrito = await _unitWork.CarritoRepo.ObtenerCarritoDeUsuario(_userManager.GetUserId(User), baseUrl);

            var carritoProductos = carrito.CarritoProductos;

            var productos = new List<Producto>();

            foreach (var carritoProducto in carritoProductos)
            {
                productos.Add(carritoProducto.Producto);
            }

            // verificar que el carrito tenga productos
            if (!productos.Any())
            {
                ModelState.AddModelError("", "El carrito está vacío.");
                return View(carritoProductos);
            }

            if (formPedido.TipoPedido.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Tipo de pedido es requerido.");
            }

            // crear pedido verificando los campos y su tipo
            var pedido = new Pedido
            {
                OwnerId = _userManager.GetUserId(User),
                DetallePedidos = new List<DetallePedido>(),
                FechaPedido = DateTime.Now,
                TipoPedido = formPedido.TipoPedido,
                EstadoPedido = VCG.EstadoPedido.Pendiente,
            };
            if  (formPedido.TipoPedido == VCG.TipoPedido.Delivery)
            {
               
                if(string.IsNullOrEmpty(formPedido.Telefono))
                        {
                    ModelState.AddModelError("", "Teléfono es requerido.");
                    return View(carritoProductos);
                }
                if (string.IsNullOrEmpty(formPedido.Direccion))
                {
                    ModelState.AddModelError("", "Dirección es requerida.");
                    return View(carritoProductos);
                }
                if (string.IsNullOrEmpty(formPedido.Referencia))
                {
                    ModelState.AddModelError("", "Referencia es requerida.");
                    return View(carritoProductos);
                }
                pedido.Telefono = formPedido.Telefono;
                pedido.Direccion = formPedido.Direccion;
                pedido.Referencia = formPedido.Referencia;
            }
            if (formPedido.TipoPedido == VCG.TipoPedido.Recoger)
            {
                if (!formPedido.FechaRecogo.HasValue)
                {
                    ModelState.AddModelError("", "Fecha de recogo es requerida.");
                    return View(carritoProductos);
                }
              
                pedido.FechaRecogo = formPedido.FechaRecogo;
            }
            if (formPedido.TipoPedido == VCG.TipoPedido.Mesa)
            {
                if (string.IsNullOrEmpty(formPedido.Mesa))
                {
                    ModelState.AddModelError("", "Mesa es requerida.");
                    return View(carritoProductos);
                }
                //requirido
                pedido.Mesa = formPedido.Mesa;
            }
          



            // crear detalles de pedido
            foreach (var carritoProducto in carritoProductos)
            {
                var detallePedido = new DetallePedido
                {
                    PedidoId = pedido.PedidoId,
                    ProductoId = carritoProducto.ProductoId,
                    Cantidad = carritoProducto.Cantidad,
           
                };

                // agregar detalles de pedido al pedido
                pedido.DetallePedidos.Add(detallePedido);
            }

            // guardar pedido
            await _unitWork.PedidoRepo.AgregarAsync(pedido);
            await _unitWork.GuardarPedido();

            //eliminar los productos del carrito
            await _unitWork.CarritoRepo.VaciarCarrito(_userManager.GetUserId(User));

            // redireccionar a index
            return RedirectToAction(nameof(Index));
        }

      

      
        // GET: PedidodsController/Delete/5
        public ActionResult Anular(int id)
        {
            return View();
        }

        // POST: PedidodsController/Delete/5
        [Authorize]
        [ValidateAntiForgeryToken]
        //only admin
        [HttpPost]
        public ActionResult AnularPedido(int id, IFormCollection collection)
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
