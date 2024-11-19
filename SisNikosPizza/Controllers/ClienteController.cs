using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Models;
using SisNikosPizza.Repository.Interfaces;
using System.Diagnostics;


namespace SisNikosPizza.Controllers
{
    public class ClienteController : Controller
    {
        //private readonly HotelElCieloDbContext _context;
        private readonly IUniwork _unitWork;

        public ClienteController(

            IUniwork unitWork
            )
        {
            //_context = context;
            _unitWork = unitWork;
        }


        public async Task<IActionResult> Index()
        {

            var Categorias = await _unitWork.CategoriaRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.CategoriaId));
            return View(Categorias);
        }

    }
}
