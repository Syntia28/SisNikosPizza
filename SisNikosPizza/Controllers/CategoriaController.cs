using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza.Controllers
{
    public class CategoriaController : Controller
    {
        //private readonly HotelElCieloDbContext _context;
        private readonly IUniwork _unitWork;

        public CategoriaController(
            
            IUniwork unitWork
            )
        {
            //_context = context;
            _unitWork = unitWork;
        }

        // Categories/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var categories = await _unitWork.CategoriaRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.CategoriaId));
            return View(categories);
        }

        // Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria category)
        {
            if (ModelState.IsValid) // Si todos los datos enviados son correctos
            {
                category.CreatedAt = DateTime.Now;
                category.UpdatedAt = DateTime.Now;
                //await _context.Category.AddAsync(category);
                //await _context.SaveChangesAsync();
                await _unitWork.CategoriaRepo.AgregarAsync(category);
                await _unitWork.GuardarCategoria();
                TempData[VCG.Satisfactorio] = "Categoría actualizada correctamente";
                //return RedirectToAction("Index");
                //return RedirectToAction("Details", new(category.CategoryId.ToString()));
                return RedirectToAction("Details", new { id = category.CategoriaId });
            }

            // En caso de errores se retorna a la vista
            TempData[VCG.Errado] = "No se pudo guardar la categoría, intente de nuevo.";
            return View(category);

        }

        // Categories/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            
            var category = await _unitWork.CategoriaRepo.ObtenerAsync(id);
            if (category is null)
                return NotFound();

            return View(category);
        }

        // Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Categoria category)
        {
            if (category is null) return NotFound();// Si todos los datos enviados son correctos
            {
               
               

                _unitWork.CategoriaRepo.ActualizarCategoria(category);
                await _unitWork.GuardarCategoria();
                TempData[VCG.Satisfactorio] = "Categoría actualizada correctamente.";
                return RedirectToAction("Index");
            }

            // En caso de errores se retorna a la vista
            TempData[VCG.Errado] = "No se pudo actualizar la categoría, intente de nuevo.";
            return View(category);
        }

        // Categories/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var category = await _unitWork.CategoriaRepo.ObtenerPrimeroAsync(filtro: c => c.CategoriaId == id);
            return View(category);
        }

        // Categories/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //var category = await _context.Category.FindAsync(id);
            var category = await _unitWork.CategoriaRepo.ObtenerAsync(id);

            if (category is null)
                return NotFound();

            return View(category);
        }

        // Categories/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Categoria category)
        {
            if (category is not null)
            {
                //_context.Category.Remove(category);
                //await _context.SaveChangesAsync();
                _unitWork.CategoriaRepo.Eliminar(category);
                await _unitWork.GuardarCategoria();
                return RedirectToAction("Index");
            }

            // En caso de errores se retorna a la vista
            return View(category);

        }
    }
}
