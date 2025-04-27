using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;

namespace SisNikosPizza.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IUniwork _unitWork;
         private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(
            //HotelElCieloDbContext context,
            IUniwork unitWork,
            IWebHostEnvironment webHostEnvironment
            )
        {
            //_context = context;
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

            // Construir la URL completa para cada producto
            string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
            foreach (var producto in productos)
            {
                if (!string.IsNullOrEmpty(producto.ImagenUrl))
                {
                    producto.ImagenUrl = $"{baseUrl}/{producto.ImagenUrl}";
                }
            }

            return View(productos);
        }

        // Categories/Create
        // Rooms/Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoriesList = (await _unitWork.CategoriaRepo.ObtenerTodosAsync())
                .Select(c => new SelectListItem { Text = c.nombre, Value = c.CategoriaId.ToString() });
            ViewBag.InsumosList = await _unitWork.InsumoRepo.ObtenerTodosAsync(i => i.Estado == true);
            return View();
        }

   


            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Producto producto, IFormFile imageFile, int[] insumoIds, float[] insumoCantidades)
            {
           Producto newProduct = new Producto();

            // Validar imagen
            if (imageFile == null || imageFile.Length == 0)
                {
                    ModelState.AddModelError("ImagenUrl", "La imagen es obligatoria.");
                }
            // Validar que no se repita el nombre del producto 
            var productoExistente = await _unitWork.ProductoRepo
                .ObtenerPrimeroAsync(p => p.Nombre.ToLower() == producto.Nombre.ToLower());

            if (productoExistente != null)
            {
                ModelState.AddModelError("Nombre", "Ya existe un producto con ese nombre.");
            }
            // Validar insumos
            if (insumoIds == null || insumoIds.Length == 0 || insumoCantidades == null || insumoCantidades.Length == 0 || insumoIds.Length != insumoCantidades.Length)
                {
                    ModelState.AddModelError("", "Debes añadir al menos un insumo con una cantidad válida.");
                }

                // Validar que los insumoIds existan en la base de datos
                if (insumoIds != null && insumoIds.Length > 0)
                {
                    var validInsumoIds = (await _unitWork.InsumoRepo.ObtenerTodosAsync(i => i.Estado == true))
                        .Select(i => i.InsumoId)
                        .ToList();
                    if (insumoIds.Any(id => !validInsumoIds.Contains(id)))
                    {
                        ModelState.AddModelError("", "Uno o más insumos seleccionados no son válidos.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.CategoriesList = (await _unitWork.CategoriaRepo.ObtenerTodosAsync())
                        .Select(c => new SelectListItem { Text = c.nombre, Value = c.CategoriaId.ToString() });
                    ViewBag.InsumosList = await _unitWork.InsumoRepo.ObtenerTodosAsync(i => i.Estado == true);
                    TempData["Error"] = "No se pudo guardar el producto. Verifica los datos ingresados.";
                    return View(producto);
                }

                try
                {
                    // Guardar la imagen
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
                        producto.ImagenUrl = uniqueFileName;
                    }

                    // Configurar propiedades del producto
                    producto.Stock = "1 Unidad";
                    producto.CreatedAt = DateTime.Now;
                    producto.UpdatedAt = DateTime.Now;

                //newProduct.ProductoInsumos = productoInsumos;
                newProduct.CategoriaId = producto.CategoriaId;
                newProduct.Nombre = producto.Nombre;
                newProduct.Descripcion = producto.Descripcion;
                newProduct.Precio = producto.Precio;
                newProduct.Estado = true;
                newProduct.ImagenUrl = producto.ImagenUrl;
                newProduct.Stock = "1 unidad";

              

                // Guardar el producto y sus insumos en una sola transacción
                await _unitWork.ProductoRepo.AgregarAsync(newProduct);
                await _unitWork.GuardarProducto();

                // Agregar insumos al producto
                if (insumoIds != null && insumoCantidades != null && insumoIds.Length == insumoCantidades.Length)
                {
                    for (int i = 0; i < insumoIds.Length; i++)
                    {
                        if (insumoIds[i] > 0 && insumoCantidades[i] > 0)
                        {
                            var pi = new ProductoInsumo
                            {
                                InsumoId = insumoIds[i],
                                ProductoId = newProduct.ProductoId,
                                Cantidad = insumoCantidades[i]

                            };

                            await _unitWork.ProductoInsumoRepo.AgregarAsync(pi);
                            await _unitWork.GuardarProductoInsumo();
                        }
                    }
                }

                    TempData["Satisfactorio"] = "Producto creado correctamente.";
                    return RedirectToAction("Details", new { id = newProduct.ProductoId });
                }
                catch (Exception ex)
                {
                    // Incluir detalles de la inner exception para diagnóstico
                    var errorMessage = ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMessage += $" | Inner Exception: {ex.InnerException.Message}";
                    }
                    TempData["Error"] = $"Ocurrió un error al guardar el producto: {errorMessage}";
                    ViewBag.CategoriesList = (await _unitWork.CategoriaRepo.ObtenerTodosAsync())
                        .Select(c => new SelectListItem { Text = c.nombre, Value = c.CategoriaId.ToString() });
                    ViewBag.InsumosList = await _unitWork.InsumoRepo.ObtenerTodosAsync(i => i.Estado == true);
                    return View(producto);
                }
            }
       

    // Categories/Edit/5
    [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //if (id <= 0)
            //    return NotFound();
   
            var producto = await _unitWork.ProductoRepo.ObtenerAsync(id);
            if (producto is null)
                return NotFound();


            var categories = await _unitWork.CategoriaRepo.ObtenerTodosAsync();
            IEnumerable<SelectListItem> categoriesList = categories.Select(c => new SelectListItem
            {
                Text = c.nombre,
                Value = c.CategoriaId.ToString()
            }).ToList();


            // Si hay errores de validación, regresar a la vista
            ViewBag.CategoriesList = categoriesList;


            return View(producto);
        }

        // producto/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Producto productoForm, IFormFile imageFile)
        {


            if (ModelState.IsValid)
            {
                // cargar el producto existente de la base de datos
                var productoDB = await _unitWork.ProductoRepo.ObtenerAsync(productoForm.ProductoId);

                if (productoDB == null) return NotFound();

              
                productoForm.UpdatedAt = DateTime.Now;

                //actualizar propiedades del producto
                productoDB.Nombre= productoForm.Nombre;
                productoDB.Descripcion = productoForm.Descripcion;
                productoDB.Descripcion = productoForm.Descripcion;
                productoDB.CategoriaId = productoForm.CategoriaId;
                productoDB.Precio = productoForm.Precio;
                productoDB.Stock = productoForm.Stock;
                productoDB.Estado = productoForm.Estado;

                // Manejar la subida de imagen
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Crear el directorio si no existe
                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Guardar solo el nombre del archivo en la base de datos
                  
                    productoDB.ImagenUrl = uniqueFileName;
                }


                // Actualizar los datos del producto en la base de datos
                await _unitWork.GuardarProducto();
                return RedirectToAction("Index");
            }


            var categories = await _unitWork.CategoriaRepo.ObtenerTodosAsync();
            IEnumerable<SelectListItem> categoriesList = categories.Select(c => new SelectListItem
            {
                Text = c.nombre,
                Value = c.CategoriaId.ToString()
            }).ToList();


            // Si hay errores de validación, regresar a la vista
            ViewBag.CategoriesList = categoriesList;


            return View(productoForm);
        }

        // producto/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var producto = await _unitWork.ProductoRepo.ObtenerPrimeroAsync(filtro: c => c.ProductoId == id);
            if (producto == null) return NotFound();

            // Construir la URL completa de la imagen
            if (!string.IsNullOrEmpty(producto.ImagenUrl))
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
                producto.ImagenUrl = $"{baseUrl}/{producto.ImagenUrl}";
            }

            return View(producto);
        }
        // Categories/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //var category = await _context.Category.FindAsync(id);
            if (id is null) return NotFound();

            var producto = await _unitWork.ProductoRepo.ObtenerPrimeroAsync(filtro: c => c.ProductoId == id);
            if (producto == null) return NotFound();

            // Construir la URL completa de la imagen
            if (!string.IsNullOrEmpty(producto.ImagenUrl))
            {
                string baseUrl = $"{Request.Scheme}://{Request.Host}/images";
                producto.ImagenUrl = $"{baseUrl}/{producto.ImagenUrl}";
            }

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
