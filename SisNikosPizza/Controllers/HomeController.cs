using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.ViewModels;
using SisNikosPizza.Repository.Interfaces;
using System.Diagnostics;

namespace SisNikosPizza.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUniwork _unitWork;

        public HomeController(IUniwork unitWork,ILogger<HomeController> logger)
        {
            _logger = logger;
            _unitWork = unitWork;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productos = await _unitWork.ProductoRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.ProductoId));

            // Construir la URL completa para cada producto
            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            foreach (var producto in productos)
            {
                if (!string.IsNullOrEmpty(producto.ImagenUrl))
                {
                    producto.ImagenUrl = $"{baseUrl}/{producto.ImagenUrl}";
                }
            }
            
            return View(productos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
