using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var insumos = await _unitWork.InsumoRepo.ObtenerTodosAsync(
                incluirPropiedades: "Proveedor", 
                ordenarPor: c => c.OrderByDescending(c => c.InsumoId)
            );
            return View(insumos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var proveedores = await _unitWork.ProveedorRepo.ObtenerTodosAsync();
            ViewBag.Proveedores = new SelectList(proveedores, "ProveedorId", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Insumo insumo)
        {
            insumo.CreatedAt = DateTime.Now;
            insumo.UpdatedAt = DateTime.Now;

            // Validación de nombre duplicado
            var existeNombre = (await _unitWork.InsumoRepo.ObtenerTodosAsync())
                .Any(i => i.Nombre.Trim().ToLower() == insumo.Nombre.Trim().ToLower());
            if (existeNombre)
            {
                ModelState.AddModelError("Nombre", "Ya existe un insumo con ese nombre.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var proveedor = await _unitWork.ProveedorRepo.ObtenerAsync(insumo.ProveedorId);
                    if (proveedor == null)
                    {
                        ModelState.AddModelError("ProveedorId", "El proveedor seleccionado no existe.");
                        var proveedores = await _unitWork.ProveedorRepo.ObtenerTodosAsync();
                        ViewBag.Proveedores = new SelectList(proveedores, "ProveedorId", "Nombre");
                        return View(insumo);
                    }

                    await _unitWork.InsumoRepo.AgregarAsync(insumo);
                    await _unitWork.GuardarAsync();
                    TempData["Satisfactorio"] = "Insumo creado correctamente.";
                    return RedirectToAction("Details", new { id = insumo.InsumoId });
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Ocurrió un error al guardar el insumo: {ex.Message}";
                }
            }

            var proveedoresReload = await _unitWork.ProveedorRepo.ObtenerTodosAsync();
            ViewBag.Proveedores = new SelectList(proveedoresReload, "ProveedorId", "Nombre", insumo.ProveedorId);

            TempData["Error"] = "No se pudo guardar el insumo. Verifica los datos ingresados.";
            return View(insumo);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var insumo = await _unitWork.InsumoRepo.ObtenerAsync(id);
            if (insumo is null)
                return NotFound();

            var proveedores = await _unitWork.ProveedorRepo.ObtenerTodosAsync();
            ViewBag.Proveedores = new SelectList(proveedores, "ProveedorId", "Nombre", insumo.ProveedorId);

            return View(insumo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Insumo insumoForm)
        {
            if (ModelState.IsValid)
            {
                insumoForm.UpdatedAt = DateTime.Now;

                try
                {
                    _unitWork.InsumoRepo.Actualizar(insumoForm);
                    await _unitWork.GuardarAsync();
                    TempData["Satisfactorio"] = "Insumo actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Ocurrió un error al actualizar el insumo: {ex.Message}";
                }
            }

            var proveedores = await _unitWork.ProveedorRepo.ObtenerTodosAsync();
            ViewBag.Proveedores = new SelectList(proveedores, "ProveedorId", "Nombre", insumoForm.ProveedorId);

            return View(insumoForm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var insumo = await _unitWork.InsumoRepo.ObtenerPrimeroAsync(
                filtro: c => c.InsumoId == id,
                incluirPropiedades: "Proveedor"
            );

            if (insumo == null) return NotFound();
            return View(insumo);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var insumo = await _unitWork.InsumoRepo.ObtenerPrimeroAsync(
                filtro: c => c.InsumoId == id,
                incluirPropiedades: "Proveedor"
            );

            if (insumo == null) return NotFound();
            return View(insumo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Insumo insumo)
        {
            if (insumo is not null)
            {
                try
                {
                    var insumoEnBD = await _unitWork.InsumoRepo.ObtenerAsync(insumo.InsumoId);
                    if (insumoEnBD is null)
                    {
                        TempData["Error"] = "El insumo no fue encontrado.";
                        return RedirectToAction("Index");
                    }

                    _unitWork.InsumoRepo.Eliminar(insumoEnBD);
                    await _unitWork.GuardarAsync();
                    TempData["Satisfactorio"] = "Insumo eliminado correctamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Ocurrió un error al eliminar el insumo: {ex.Message}";
                }
            }

            return View(insumo);
        }

    }
}