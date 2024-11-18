using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IUniwork _unitWork;

        public ProductoController(
            //HotelElCieloDbContext context,
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
            
            var producto = await _unitWork.ProductoRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.ProductoId));
            return View(producto);
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
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid) // Si todos los datos enviados son correctos
            {
                //producto.Nombre = producto.Nombre.ToString();

                producto.CreatedAt = DateTime.Now;
                producto.UpdatedAt = DateTime.Now;
               
                //await _context.Category.AddAsync(category);
                //await _context.SaveChangesAsync();
                await _unitWork.ProductoRepo.AgregarAsync(producto);
                await _unitWork.GuardarProducto();
                TempData[VCG.Satisfactorio] = "Producto actualizada correctamente";
                //return RedirectToAction("Index");
                //return RedirectToAction("Details", new(category.CategoryId.ToString()));
                return RedirectToAction("Details", new { id = producto.ProductoId });
            }

            // En caso de errores se retorna a la vista
            TempData[VCG.Errado] = "No se pudo guardar el producto, intente de nuevo.";
            return View(producto);

        }

        // Categories/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //if (id <= 0)
            //    return NotFound();
            //var category = await _context.Category.FindAsync(id);
            var producto = await _unitWork.ProductoRepo.ObtenerAsync(id);
            if (producto is null)
                return NotFound();

            return View(producto);
        }

        // Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Producto producto)
        {
            if (producto is null) return NotFound();

            if (ModelState.IsValid) // Si todos los datos enviados son correctos
            {
                //var categoryOnDb = await _context.Category.AsNoTracking()
                //                .FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
                //category.CreatedAt = categoryOnDb.CreatedAt;

                //category.UpdatedAt = DateTime.Now;
                //_context.Category.Update(category);
                //await _context.SaveChangesAsync();
                _unitWork.ProductoRepo.Actualizar(producto);
                await _unitWork.GuardarProducto();
                TempData[VCG.Satisfactorio] = "Producto actualizada correctamente.";
                return RedirectToAction("Index");
            }

            // En caso de errores se retorna a la vista
            TempData[VCG.Errado] = "No se pudo actualizar el producto, intente de nuevo.";
            return View(producto);
        }

        // Categories/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var category = await _unitWork.ProductoRepo.ObtenerPrimeroAsync(filtro: c => c.ProductoId == id);
            return View(category);
        }

        // Categories/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //var category = await _context.Category.FindAsync(id);
            var producto= await _unitWork.ProductoRepo.ObtenerAsync(id);

            if (producto is null)
                return NotFound();

            return View(producto);
        }

        // Categories/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Producto producto)
        {
            if (producto is not null)
            {
                //_context.Category.Remove(category);
                //await _context.SaveChangesAsync();
                _unitWork.ProductoRepo.Eliminar(producto);
                await _unitWork.GuardarProducto();
                return RedirectToAction("Index");
            }

            // En caso de errores se retorna a la vista
            return View(producto);

        }
    }
}
