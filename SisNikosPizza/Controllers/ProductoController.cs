using Microsoft.AspNetCore.Mvc;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Domain.ViewModels;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IUniwork _unitWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(
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
            var productos = await _unitWork.ProductoRepo.ObtenerTodosAsync(ordenarPor: c => c.OrderByDescending(c => c.ProductoId));

            foreach (var item in productos)
            {
                var insumos = await _unitWork.ProductoInsumoRepo.ObtenerTodosAsync(filtro: p => p.ProductoId == item.ProductoId, incluirPropiedades: "Insumo");

                item.ProductoInsumos = insumos.ToList();
            }

            return View(productos);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            VMDProducto ProductoVM = new VMDProducto()
            {
                producto = new Producto(),
                CategoriasList = _unitWork.ProductoRepo.ListarCategorias("categorias"),
                InsumosList = _unitWork.ProductoRepo.ListarInsumos("insumos"),
            };

            return View(ProductoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VMDProducto VMDP)
        {
            string rutaPrincipal = _webHostEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivos.Count > 0)
            {
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"productos");

                if (!Directory.Exists(subidas)) Directory.CreateDirectory(subidas);

                var archivo = archivos[0];
                var extension = ".webp";
                var rutaCompleta = Path.Combine(subidas, nombreArchivo + extension);

                using (var stream = archivo.OpenReadStream())
                {
                    await ImageResizer.ResizeImageAsync(stream, rutaCompleta, 255);
                }

                VMDP.producto.ImagenUrl = @"\productos\" + nombreArchivo + extension;
            }

            // Validar que no exista un producto con el mismo nombre y categoría
            var existeProducto = (await _unitWork.ProductoRepo.ObtenerTodosAsync(
                filtro: p => p.Nombre.ToLower() == VMDP.producto.Nombre.ToLower() && p.CategoriaId == VMDP.producto.CategoriaId
            )).Any();

            if (existeProducto)
            {
                ModelState.AddModelError("producto.Nombre", "Ya existe un producto con el mismo nombre en esta categoría.");
            }

            if (ModelState.IsValid)
            {
                await _unitWork.ProductoRepo.AgregarAsync(VMDP.producto);
                await _unitWork.GuardarAsync();

                if (VMDP.InsumosSeleccionados != null)
                {
                    foreach (var item in VMDP.InsumosSeleccionados)
                    {
                        ProductoInsumo productoInsumo = new ProductoInsumo()
                        {
                            ProductoId = VMDP.producto.ProductoId,
                            InsumoId = item.InsumoId,
                            Cantidad = item.Cantidad
                        };
                        await _unitWork.ProductoInsumoRepo.AgregarAsync(productoInsumo);
                    }
                    await _unitWork.GuardarAsync();
                }

                TempData["Satisfactorio"] = "Producto creado correctamente.";
                return RedirectToAction("Details", new { id = VMDP.producto.ProductoId });
            }

            // Recargar listas si hay error
            VMDP.CategoriasList = _unitWork.ProductoRepo.ListarCategorias("categorias");
            VMDP.InsumosList = _unitWork.ProductoRepo.ListarInsumos("insumos");
            return View(VMDP);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {


            VMDProducto PVM = new VMDProducto()
            {
                producto = new Producto(),
                CategoriasList = _unitWork.ProductoRepo.ListarCategorias("categorias"),
                InsumosList = _unitWork.ProductoRepo.ListarInsumos("insumos"),
            };

            PVM.producto = await _unitWork.ProductoRepo.ObtenerAsync(id);

            if (PVM is null)
                return NotFound();

            return View(PVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMDProducto VMDP, IFormFile imageFile)
        {


            if (ModelState.IsValid)
            {
                var productoDB = await _unitWork.ProductoRepo.ObtenerAsync(VMDP.producto.ProductoId);

                if (productoDB == null) return NotFound();


                VMDP.producto.UpdatedAt = DateTime.Now;

                productoDB.Nombre = VMDP.producto.Nombre;
                productoDB.Descripcion = VMDP.producto.Descripcion;
                productoDB.Descripcion = VMDP.producto.Descripcion;
                productoDB.CategoriaId = VMDP.producto.CategoriaId;
                productoDB.Precio = VMDP.producto.Precio;
                productoDB.Stock = VMDP.producto.Stock;
                productoDB.Estado = VMDP.producto.Estado;

                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "productos");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    // Redimensionar la imagen a 255px de ancho manteniendo la relación de aspecto
                    using (var stream = imageFile.OpenReadStream())
                    {
                        await ImageResizer.ResizeImageAsync(stream, filePath, 255);
                    }

                    productoDB.ImagenUrl = @"\productos\" + uniqueFileName;
                }


                await _unitWork.GuardarAsync();
                return RedirectToAction("Index");
            }


            VMDP.CategoriasList = _unitWork.ProductoRepo.ListarCategorias("categorias");
            VMDP.InsumosList = _unitWork.ProductoRepo.ListarInsumos("insumos");
            return View(VMDP);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var producto = await _unitWork.ProductoRepo.ObtenerPrimeroAsync(filtro: c => c.ProductoId == id);
            if (producto == null) return NotFound();

            if (!string.IsNullOrEmpty(producto.ImagenUrl))
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}";
                producto.ImagenUrl = $"{baseUrl}/{producto.ImagenUrl}";
            }

            return View(producto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var producto = await _unitWork.ProductoRepo.ObtenerPrimeroAsync(filtro: c => c.ProductoId == id);
            if (producto == null) return NotFound();

            if (!string.IsNullOrEmpty(producto.ImagenUrl))
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
                producto.ImagenUrl = $"{baseUrl}/{producto.ImagenUrl}";
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Producto producto)
        {
            if (producto is not null)
            {
                _unitWork.ProductoRepo.Eliminar(producto);
                await _unitWork.GuardarAsync();
                return RedirectToAction("Index");
            }

            return View(producto);

        }
    }
}
