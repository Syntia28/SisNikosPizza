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
            VMDProducto DocVM = new VMDProducto()
            {
                Doc = new Documento(),
                EstadosList = _UnidadDeTrabajo.DocumentoRepo.ListarEstados("Estados"),
                CategoriaList = _UnidadDeTrabajo.DocumentoRepo.ListarCategorias("Categorias"),
                ClientesList = _UnidadDeTrabajo.DocumentoRepo.ListarClientes("Cliente")

            };
            return View(DocVM);
            ViewBag.CategoriesList = (await _unitWork.CategoriaRepo.ObtenerTodosAsync())
                .Select(c => new SelectListItem { Text = c.nombre, Value = c.CategoriaId.ToString() });
            ViewBag.InsumosList = await _unitWork.InsumoRepo.ObtenerTodosAsync(i => i.Estado == true);
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, int[] insumoIds, float[] insumoCantidades)
        {
            string rutaPrincipal = _webHostEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivos.Count > 0)
            {
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\");

                if (!Directory.Exists(subidas)) Directory.CreateDirectory(subidas);

                var archivo = archivos[0];
                var extension = Path.GetExtension(archivo.FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create)) await archivo.CopyToAsync(fileStreams);

                producto.ImagenUrl = @"\imagenes\" + nombreArchivo + extension;
            }

            if (ModelState.IsValid)
            {
                await _unitWork.ProductoRepo.AgregarAsync(producto);
                await _unitWork.GuradarAsync();

                TempData["Satisfactorio"] = "Producto creado correctamente.";
                return RedirectToAction("Details", new { id = producto.ProductoId });
            }
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
                await _unitWork.GuradarAsync();
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
                await _unitWork.GuradarAsync();
                return RedirectToAction("Index");
            }

            // En caso de errores se retorna a la vista
            return View(producto);

        }
    }
}
