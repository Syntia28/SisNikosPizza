using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Models;
using SisNikosPizza.Repository.Interfaces;

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
        // GET: PedidodsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PedidodsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        [Authorize]
        public async Task< ActionResult> New(int? addProduct)
        {
            var productosDisponibles = await _unitWork.ProductoRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.ProductoId));
         
            var loggedInUser= await _userManager.GetUserAsync(User);

            List<Producto> orderedProducts = new List<Producto>();

            // GET: PedidodsController/Create
           
                if (addProduct.HasValue)
                {
                    var product = _unitWork.ProductoRepo.ObtenerAsync(addProduct.Value);
                if (product != null)
                    {
                    // create pedido
                    var pedido = new Pedido();
                    pedido.FechaPedido = DateTime.Now;
                    
                    pedido.OwnerId = loggedInUser?.Id != null ? loggedInUser.Id : "";

                    //guardar pedido
                    await _unitWork.PedidoRepo.AgregarAsync(pedido);
                    await _unitWork.GuardarPedido();

                }

                }
                var baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            var Pedidos = new List<Pedido>();

            var model = new PedidoVistaModel
            {
                ProductosDisponibles = productosDisponibles as List<Producto>,
                Pedidos = Pedidos as List<Pedido>
            };
            return View(model);
            }
        

      
  

        // POST: PedidodsController/Create
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

        // GET: PedidodsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PedidodsController/Edit/5
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

        // GET: PedidodsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PedidodsController/Delete/5
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
