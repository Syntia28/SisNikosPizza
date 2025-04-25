using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

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

            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            foreach (var insumo in insumos)
            {
                if (!string.IsNullOrEmpty(insumo.ImagenUrl))
                {
                    insumo.ImagenUrl = $"{baseUrl}/{insumo.ImagenUrl}";
                }
            }

            return View(insumos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Insumo insumo, IFormFile imageFile)
        {
            Console.WriteLine($"Nombre: {insumo.Nombre}");
            Console.WriteLine($"Precio: {insumo.Precio}");
            Console.WriteLine($"Imagen recibida: {(imageFile != null ? "Sí" : "No")}");

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        Directory.CreateDirectory(uploadsFolder);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        insumo.ImagenUrl = uniqueFileName;
                    }

                    insumo.CreatedAt = DateTime.Now;
                    insumo.UpdatedAt = DateTime.Now;

                    await _unitWork.InsumoRepo.AgregarAsync(insumo);
                    await _unitWork.GuardarInsumo();

                    TempData["Satisfactorio"] = "Insumo creado correctamente.";
                    return RedirectToAction("Details", new { id = insumo.InsumoId });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar el insumo: {ex.Message}");
                    TempData["Error"] = "Ocurrió un error al guardar el insumo. Intenta nuevamente.";
                }
            }

            foreach (var key in ModelState.Keys)
            {
                Console.WriteLine($"{key}: {string.Join(", ", ModelState[key].Errors.Select(e => e.ErrorMessage))}");
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
        public async Task<IActionResult> Edit(Insumo insumoForm, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var insumoDB = await _unitWork.InsumoRepo.ObtenerAsync(insumoForm.InsumoId);
                if (insumoDB == null) return NotFound();

                insumoForm.UpdatedAt = DateTime.Now;

                insumoDB.Nombre = insumoForm.Nombre;
                insumoDB.Descripcion = insumoForm.Descripcion;
                insumoDB.Precio = insumoForm.Precio;
                insumoDB.UnidadeDeMedida = insumoForm.UnidadeDeMedida;
                insumoDB.Cantidad = insumoForm.Cantidad;
                insumoDB.Estado = insumoForm.Estado;

                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    insumoDB.ImagenUrl = uniqueFileName;
                }

                await _unitWork.GuardarInsumo();
                return RedirectToAction("Index");
            }

            return View(insumoForm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var insumo = await _unitWork.InsumoRepo.ObtenerPrimeroAsync(filtro: c => c.InsumoId == id);
            if (insumo == null) return NotFound();

            if (!string.IsNullOrEmpty(insumo.ImagenUrl))
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
                insumo.ImagenUrl = $"{baseUrl}/{insumo.ImagenUrl}";
            }

            return View(insumo);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var insumo = await _unitWork.InsumoRepo.ObtenerPrimeroAsync(filtro: c => c.InsumoId == id);
            if (insumo == null) return NotFound();

            if (!string.IsNullOrEmpty(insumo.ImagenUrl))
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
                insumo.ImagenUrl = $"{baseUrl}/{insumo.ImagenUrl}";
            }

            return View(insumo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Insumo insumo)
        {
            if (insumo is not null)
            {
                _unitWork.InsumoRepo.Eliminar(insumo);
                await _unitWork.GuardarInsumo();
                return RedirectToAction("Index");
            }

            return View(insumo);
        }
    }
}