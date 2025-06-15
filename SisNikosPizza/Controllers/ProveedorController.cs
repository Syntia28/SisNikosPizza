using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;
using System.Threading.Tasks;

namespace SisNikosPizza.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IUniwork _unitWork;

        public ProveedorController(IUniwork unitWork)
        {
            _unitWork = unitWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var proveedores = await _unitWork.ProveedorRepo.ObtenerTodosAsync(
                ordenarPor: p => p.OrderByDescending(p => p.ProveedorId));
            return View(proveedores);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                proveedor.CreatedAt = DateTime.Now;
                proveedor.UpdatedAt = DateTime.Now;

                await _unitWork.ProveedorRepo.AgregarAsync(proveedor);
                await _unitWork.GuardarAsync();

                TempData[VCG.Satisfactorio] = "Proveedor guardado correctamente.";
                return RedirectToAction("Details", new { id = proveedor.ProveedorId });
            }

            TempData[VCG.Errado] = "Error al guardar el proveedor. Intente de nuevo.";
            return View(proveedor);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var proveedor = await _unitWork.ProveedorRepo.ObtenerAsync(id);
            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

            proveedor.UpdatedAt = DateTime.Now;

            _unitWork.ProveedorRepo.ActualizarProveedor(proveedor);
            await _unitWork.GuardarAsync();

            TempData[VCG.Satisfactorio] = "Proveedor actualizado correctamente.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _unitWork.ProveedorRepo.ObtenerPrimeroAsync(
                filtro: p => p.ProveedorId == id);
            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _unitWork.ProveedorRepo.ObtenerAsync(id);
            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Proveedor proveedor)
        {
            if (proveedor != null)
            {
                _unitWork.ProveedorRepo.Eliminar(proveedor);
                await _unitWork.GuardarAsync();

                TempData[VCG.Satisfactorio] = "Proveedor eliminado correctamente.";
                return RedirectToAction("Index");
            }

            TempData[VCG.Errado] = "Error al eliminar el proveedor.";
            return View(proveedor);
        }
    }
}
