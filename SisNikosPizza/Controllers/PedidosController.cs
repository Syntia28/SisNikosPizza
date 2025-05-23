using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        // GET: para la vista del delivery
        public async Task<ActionResult> Delivery()
        {
            var pedidosDelivery = await _unitWork.DetallesPedidoRepo.ObtenerTodosAsync(
                filtro: p => p.Pedido.TipoPedido == VCG.TipoPedido.Delivery,
                ordenarPor: p => p.OrderByDescending(p => p.Pedido.CreatedAt),
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
        [HttpGet]
        public async Task<ActionResult> Nuevo(string TipoPedido)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            var carrito = await _unitWork.CarritoRepo.ObtenerCarritoDeUsuario(_userManager.GetUserId(User),baseUrl);


           var carritoProductos = carrito.CarritoProductos;
            var nuevoPedidoVista = new NuevoPedidoVista();
            {
                CarritoProductos = carritoProductos.ToList();
                Pedido = new Pedido();
            };
            ViewBag.TipoPedido = TipoPedido;
            return View(nuevoPedidoVista);
        }





        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CrearPedido(Pedido formPedido)
        {
            // Obtener el carrito del usuario
            var userId = _userManager.GetUserId(User);
            var carrito = await _unitWork.CarritoRepo.ObtenerCarritoDeUsuario(userId, "");
            var carritoProductos = carrito.CarritoProductos;

            // Validar si el carrito está vacío
            if (!carritoProductos.Any())
            {
                TempData["ErrorCarroVacio"] = "El carrito está vacío.";
                return RedirectToAction("Nuevo", new { TipoPedido = formPedido.TipoPedido });
            }

            // Validar si el tipo de pedido es nulo o vacío
            if (string.IsNullOrEmpty(formPedido.TipoPedido))
            {
                TempData["ErrorTipoPedido"] = "Tipo de pedido es requerido.";
                return RedirectToAction("Nuevo", new { TipoPedido = formPedido.TipoPedido });
            }

            // Crear el pedido
            var pedido = new Pedido
            {
                UserId = userId,
                DetallePedido = new List<DetallePedido>(),
                FechaPedido = DateTime.Now,
                TipoPedido = formPedido.TipoPedido,
                EstadoPedido = VCG.EstadoPedido.Pendiente
            };

            // Validar campos específicos del tipo de pedido
            if (formPedido.TipoPedido == VCG.TipoPedido.Delivery)
            {
                if (string.IsNullOrEmpty(formPedido.Telefono))
                    return RedirectToAction("Nuevo", new { TipoPedido = formPedido.TipoPedido });
                if (string.IsNullOrEmpty(formPedido.Direccion))
                    return RedirectToAction("Nuevo", new { TipoPedido = formPedido.TipoPedido });
                if (string.IsNullOrEmpty(formPedido.Referencia))
                    return RedirectToAction("Nuevo", new { TipoPedido = formPedido.TipoPedido });



                pedido.Telefono = formPedido.Telefono;
                pedido.Direccion = formPedido.Direccion;
                pedido.Referencia = formPedido.Referencia;
            }
            else if (formPedido.TipoPedido == VCG.TipoPedido.Recoger)
            {
                if (!formPedido.FechaRecogo.HasValue)
                {
                    TempData["ErrorFechaRecogo"] = "Fecha de recogo es requerida.";
                    return RedirectToAction("Nuevo", new { TipoPedido = formPedido.TipoPedido });
                }
                pedido.FechaRecogo = formPedido.FechaRecogo;
            }
            else if (formPedido.TipoPedido == VCG.TipoPedido.Mesa)
            {
                if (string.IsNullOrEmpty(formPedido.Mesa))
                {
                    TempData["ErrorMesa"] = "Mesa es requerida.";
                    return RedirectToAction("Nuevo", new { TipoPedido = formPedido.TipoPedido });
                }
                pedido.Mesa = formPedido.Mesa;
            }

            // Crear los detalles del pedido usando IEnumerable
            pedido.DetallePedido = carritoProductos.Select(carritoProducto => new DetallePedido
            {
                ProductoId = carritoProducto.ProductoId,
                Cantidad = carritoProducto.Cantidad
            }).ToList();

            // Guardar el pedido
            await _unitWork.PedidoRepo.AgregarAsync(pedido);
            await _unitWork.GuradarAsync();

            // Vaciar el carrito del usuario
            await _unitWork.CarritoRepo.VaciarCarrito(userId);

            // Redirigir al índice de pedidos
            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = VCG.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult> Cobrar(int id)
        {

            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";

            //obtener el pedido de un usuario
            Pedido pedido = await _unitWork.PedidoRepo.ObtenerPedidoDetallado(id, baseUrl);

            if (pedido == null)
            {
                return NotFound();
            }

            CobrarPedidoModel cobrarPedidoModel = new CobrarPedidoModel()
            {
                Pedido = pedido
            };
            return View(cobrarPedidoModel);
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
