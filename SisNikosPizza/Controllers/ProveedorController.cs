using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;
using System.Diagnostics;


namespace SisNikosPizza.Controllers
{
    public class ProveedorController: Controller
    {
        private readonly IUniwork _unitWork;

        public ProveedorController(

            IUniwork unitWork
            )
        {
            _unitWork = unitWork;
        }

        // Categories/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var proveedores = await _unitWork.ProveedorRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.ProveedorId));
            return View(proveedores);
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
        public async Task<IActionResult> Create(Proveedor proveedores)
        {
            if (ModelState.IsValid) // Si todos los datos enviados son correctos
            {
                proveedores.CreatedAt = DateTime.Now;
                proveedores.UpdatedAt = DateTime.Now;
                //await _context.Category.AddAsync(category);
                //await _context.SaveChangesAsync();
                await _unitWork.ProveedorRepo.AgregarAsync(proveedores);
                await _unitWork.GuradarAsync();
                TempData[VCG.Satisfactorio] = "Proveedor actualizadado correctamente";
                //return RedirectToAction("Index");
                //return RedirectToAction("Details", new(category.CategoryId.ToString()));
                return RedirectToAction("Details", new { id = proveedores.ProveedorId });
            }

            // En caso de errores se retorna a la vista
            TempData[VCG.Errado] = "No se pudo guardar el proveedor, intente de nuevo.";
            return View(proveedores);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var proveedores = await _unitWork.ProveedorRepo.ObtenerAsync(id);
            if (proveedores is null)
                return NotFound();

            return View(proveedores);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Proveedor proveedores)
        {
            if (proveedores is null) return NotFound();// Si todos los datos enviados son correctos
            {



                _unitWork.ProveedorRepo.ActualizarProveedor(proveedores);
                await _unitWork.GuradarAsync();
                TempData[VCG.Satisfactorio] = "Proveedor actualizado correctamente.";
                return RedirectToAction("Index");
            }
        }

        // Categories/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var proveedores = await _unitWork.ProveedorRepo.ObtenerPrimeroAsync(filtro: c => c.ProveedorId == id);
            return View(proveedores);
        }

        // Categories/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //var category = await _context.Category.FindAsync(id);
            var proveedores = await _unitWork.ProveedorRepo.ObtenerAsync(id);

            if (proveedores is null)
                return NotFound();

            return View(proveedores);
        }

        // Categories/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Proveedor proveedores)
        {
            if (proveedores is not null)
            {
                //_context.Category.Remove(category);
                //await _context.SaveChangesAsync();
                _unitWork.ProveedorRepo.Eliminar(proveedores);
                await _unitWork.GuradarAsync();
                return RedirectToAction("Index");
            }

            // En caso de errores se retorna a la vista
            return View(proveedores);

        }
    }
}
