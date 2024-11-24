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
            // Enviar la lista de categorias
            var categories = await _unitWork.CategoriaRepo.ObtenerTodosAsync();
            IEnumerable<SelectListItem> categoriesList = categories.Select(c => new SelectListItem
            {
                Text = c.nombre,
                Value = c.CategoriaId.ToString()
            }).ToList();
            // select CategoryId, Name from Categories
            // _context.Category.ToList();
            // ViewData, ViewBag
            //ViewData["CategoriesList"] = categoriesList;
            ViewBag.CategoriesList = categoriesList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, IFormFile imageFile)
        {
            // Depuración: Verificar los valores recibidos
            Console.WriteLine($"CategoriaId: {producto.CategoriaId}");
            Console.WriteLine($"Nombre: {producto.Nombre}");
            Console.WriteLine($"Precio: {producto.Precio}");
            Console.WriteLine($"Imagen recibida: {(imageFile != null ? "Sí" : "No")}");

            // Validar si se subió una imagen
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ImagenUrl", "La imagen es obligatoria.");
            }

            // Validar el estado del modelo
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Modelo no válido");
                foreach (var key in ModelState.Keys)
                {
                    Console.WriteLine($"{key}: {string.Join(", ", ModelState[key].Errors.Select(e => e.ErrorMessage))}");
                }

                // Obtener las categorías nuevamente para la vista
                var categories = await _unitWork.CategoriaRepo.ObtenerTodosAsync();
                ViewBag.CategoriesList = categories.Select(c => new SelectListItem
                {
                    Text = c.nombre,
                    Value = c.CategoriaId.ToString()
                }).ToList();

                TempData["Error"] = "No se pudo guardar el producto. Verifica los datos ingresados.";
                return View(producto);
            }

            try
            {
                // Guardar la imagen si se subió
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
                    producto.ImagenUrl = uniqueFileName;
                }

                // Asignar valores predeterminados
                producto.Stock = "1 Unidad"; // O cambia esto según tu lógica
                producto.CreatedAt = DateTime.Now;
                producto.UpdatedAt = DateTime.Now;

                // Guardar el producto en la base de datos
                await _unitWork.ProductoRepo.AgregarAsync(producto);
                await _unitWork.GuardarProducto();

                TempData["Satisfactorio"] = "Producto creado correctamente.";
                return RedirectToAction("Details", new { id = producto.ProductoId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el producto: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al guardar el producto. Intenta nuevamente.";
            }

            // Si hay errores, volver a cargar las categorías y mostrar el formulario
            var categoriesFallback = await _unitWork.CategoriaRepo.ObtenerTodosAsync();
            ViewBag.CategoriesList = categoriesFallback.Select(c => new SelectListItem
            {
                Text = c.nombre,
                Value = c.CategoriaId.ToString()
            }).ToList();

            return View(producto);
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
