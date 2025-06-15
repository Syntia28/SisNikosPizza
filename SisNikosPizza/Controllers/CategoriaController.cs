using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IUniwork _unitWork;

        public CategoriaController(

            IUniwork unitWork
            )
        {
            _unitWork = unitWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var Categorias = await _unitWork.CategoriaRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.CategoriaId));
            return View(Categorias);
        }



        [HttpGet]
        [Authorize(Roles = VCG.Role_Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categorias)
        {
            if (ModelState.IsValid) 
            {
                categorias.CreatedAt = DateTime.Now;
                categorias.UpdatedAt = DateTime.Now;
                await _unitWork.CategoriaRepo.AgregarAsync(categorias);
                await _unitWork.GuardarAsync();
                TempData[VCG.Satisfactorio] = "Categoría actualizada correctamente";
                return RedirectToAction("Details", new { id = categorias.CategoriaId });
            }

            TempData[VCG.Errado] = "No se pudo guardar la categoría, intente de nuevo.";
            return View(categorias);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var categorias = await _unitWork.CategoriaRepo.ObtenerAsync(id);
            if (categorias is null)
                return NotFound();

            return View(categorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            if (categoria is null) return NotFound();
            {
                _unitWork.CategoriaRepo.ActualizarCategoria(categoria);
                await _unitWork.GuardarAsync();
                TempData[VCG.Satisfactorio] = "Categoría actualizada correctamente.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var categorias = await _unitWork.CategoriaRepo.ObtenerPrimeroAsync(filtro: c => c.CategoriaId == id);
            return View(categorias);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var categorias = await _unitWork.CategoriaRepo.ObtenerAsync(id);

            if (categorias is null)
                return NotFound();

            return View(categorias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Categoria categorias)
        {
            if (categorias is not null)
            {
                _unitWork.CategoriaRepo.Eliminar(categorias);
                await _unitWork.GuardarAsync();
                return RedirectToAction("Index");
            }

            return View(categorias);

        }
    }
}
