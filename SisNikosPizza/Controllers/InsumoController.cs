using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;

namespace SisNikosPizza.Controllers
{
    public class InsumoController : Controller
    {
        private readonly IUniwork _unitWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InsumoController(
            IUniwork unitWork,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _unitWork = unitWork;
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var insumos = await _unitWork.InsumoRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.InsumoId));

            return View(insumos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Insumo insumo)
        {
            insumo.CreatedAt = DateTime.Now;
            insumo.UpdatedAt = DateTime.Now;

            Console.WriteLine($"estado del modelo: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {

                    await _unitWork.InsumoRepo.AgregarAsync(insumo);
                    await _unitWork.GuradarAsync();

                    TempData["Satisfactorio"] = "Insumo creado correctamente.";
                    return RedirectToAction("Details", new { id = insumo.InsumoId });
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Ocurrió un error al guardar el insumo. Intenta nuevamente.";
                }
            }

            TempData["Error"] = "No se pudo guardar el insumo. Verifica los datos ingresados.";
            return View(insumo);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var insumo = await _unitWork.InsumoRepo.ObtenerAsync(id);
            if (insumo is null)
                return NotFound();

            return View(insumo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Insumo insumoForm)
        {
            _unitWork.InsumoRepo.Actualizar(insumoForm);
            await _unitWork.GuradarAsync();
            TempData["Satisfactorio"] = "Insumo actualizado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var insumo = await _unitWork.InsumoRepo.ObtenerPrimeroAsync(filtro: c => c.InsumoId == id);
            if (insumo == null) return NotFound();

            return View(insumo);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var insumo = await _unitWork.InsumoRepo.ObtenerPrimeroAsync(filtro: c => c.InsumoId == id);
            if (insumo == null) return NotFound();

            return View(insumo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Insumo insumo)
        {
            if (insumo is not null)
            {
                _unitWork.InsumoRepo.Eliminar(insumo);
                await _unitWork.GuradarAsync();
                return RedirectToAction("Index");
            }

            return View(insumo);
        }
    }
}