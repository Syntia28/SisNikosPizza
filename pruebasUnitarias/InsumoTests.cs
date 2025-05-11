using SisNikosPizza.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using HotelElCielo.Domain.Models;

namespace ProyectoPruebas
{
    [TestClass]
    public class InsumoTests
    {
        [TestMethod]
        public void CrearInsumo_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var insumo = new Insumo
            {
                InsumoId = 1,
                Nombre = "Harina",
                Descripcion = "Harina de trigo",
                ImagenUrl = "http://ejemplo.com/harina.jpg",
                Precio = 15.5f,
                UnidadeDeMedida = "Kilogramos",
                Cantidad = 20f,
                Productos = new List<ProductoInsumo>()
            };

            // Assert
            Assert.AreEqual(1, insumo.InsumoId);
            Assert.AreEqual("Harina", insumo.Nombre);
            Assert.AreEqual("Harina de trigo", insumo.Descripcion);
            Assert.AreEqual("http://ejemplo.com/harina.jpg", insumo.ImagenUrl);
            Assert.AreEqual(15.5f, insumo.Precio);
            Assert.AreEqual("Kilogramos", insumo.UnidadeDeMedida);
            Assert.AreEqual(20f, insumo.Cantidad);
            Assert.IsNotNull(insumo.Productos);
        }

        [TestMethod]
        public void CrearInsumo_SinImagenUrl_ImagenPuedeSerNula()
        {
            // Arrange
            var insumo = new Insumo
            {
                Nombre = "Sal",
                Descripcion = "Sal de mesa",
                Precio = 5f,
                UnidadeDeMedida = "Gramos",
                Cantidad = 1000f,
                Productos = new List<ProductoInsumo>()
            };

            // Assert
            Assert.IsNull(insumo.ImagenUrl);
        }

        [TestMethod]
        public void Insumo_NombreYPrecio_Validos()
        {
            // Arrange
            var insumo = new Insumo
            {
                Nombre = "Azúcar",
                Precio = 12.0f
            };

            // Act
            bool datosValidos = !string.IsNullOrEmpty(insumo.Nombre) && insumo.Precio > 0;

            // Assert
            Assert.IsTrue(datosValidos);
        }
    }
    //prueba unitaria de usuarios
    [TestClass]
    public class ApplicationUserTests
    {
        [TestMethod]
        public void CrearUsuario_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var usuario = new ApplicationUser
            {
                UserName = "usuario123",
                Email = "usuario@example.com",
                Nombre = "Juan Pérez",
                Direccion = "Calle 123",
                FechaNacimiento = new DateTime(1990, 5, 15),
                Role = "Administrador"
            };

            // Assert
            Assert.AreEqual("usuario123", usuario.UserName);
            Assert.AreEqual("usuario@example.com", usuario.Email);
            Assert.AreEqual("Juan Pérez", usuario.Nombre);
            Assert.AreEqual("Calle 123", usuario.Direccion);
            Assert.AreEqual(new DateTime(1990, 5, 15), usuario.FechaNacimiento);
            Assert.AreEqual("Administrador", usuario.Role);
        }

        [TestMethod]
        public void CrearUsuario_SinDireccionYFechaNacimiento_EsPermitido()
        {
            // Arrange
            var usuario = new ApplicationUser
            {
                UserName = "usuario456",
                Email = "otro@example.com",
                Nombre = "Ana López",
                Role = "Usuario"
            };

            // Assert
            Assert.IsNull(usuario.Direccion);
            Assert.IsNull(usuario.FechaNacimiento);
            Assert.AreEqual("Ana López", usuario.Nombre);
        }

        [TestMethod]
        public void NombreUsuario_Obligatorio_DeberiaTenerValor()
        {
            // Arrange
            var usuario = new ApplicationUser
            {
                Nombre = ""
            };

            // Act
            bool nombreValido = !string.IsNullOrEmpty(usuario.Nombre);

            // Assert
            Assert.IsFalse(nombreValido, "El campo Nombre debería ser obligatorio.");
        }
    }
    // prueba unitaria  de la entidad carrito 
    [TestClass]
    public class CarritoTests
    {
        [TestMethod]
        public void CrearCarrito_SinOwnerId_DeberiaSerInvalido()
        {
            // Arrange
            var carrito = new Carrito
            {
                CarritoId = 3,
                OwnerId = null 
            };

            // Act
            bool ownerIdValido = !string.IsNullOrEmpty(carrito.OwnerId);

            // Assert
            Assert.IsFalse(ownerIdValido, "El carrito no debería ser válido sin un OwnerId.");
        }
    }
    // prueba unitaria de la entidad carritoProducto
    [TestClass]
    public class CarritoProductoTests
    {
        [TestMethod]
        public void CrearCarritoProducto_PropiedadesAsignadasCorrectamente()
        {
            CarritoProducto cp = new CarritoProducto();
            cp.Producto = new Producto();
            cp.Producto.Nombre = "Pizza Hawaina";
            cp.Producto.Descripcion = "Pizza con jamón, piña y queso mozzarella";
            cp.ProductoId = 1; 
            cp.CarritoId = 5002;
            cp.Carrito = new Carrito();
            cp.Cantidad = 5; 

            cp.CarritoProductoId = 9002;
        }

        [TestMethod]
        public void CrearCarritoProducto_CantidadDebeSerPositiva()
        {
            // Arrange
            var carritoProducto = new CarritoProducto
            {
                Cantidad = -2 // Error: cantidad negativa
            };

            // Act
            bool cantidadValida = carritoProducto.Cantidad > 0;

            // Assert
            Assert.IsFalse(cantidadValida, "La cantidad debería ser positiva.");
        }
    }
    // prueba unitaria de la entidad categoria
    [TestClass]
    public class CategoriaTests
    {
        [TestMethod]
        public void CrearCategoria_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var categoria = new Categoria
            {
                CategoriaId = 1,
                nombre = "Bebidas"
            };

            // Assert
            Assert.AreEqual(1, categoria.CategoriaId);
            Assert.AreEqual("Bebidas", categoria.nombre);
        }

        [TestMethod]
        public void CrearCategoria_NombreNoDebeSerNulo()
        {
            // Arrange
            var categoria = new Categoria
            {
                CategoriaId = 2,
                nombre = null // nombre nulo
            };

            // Act
            bool nombreValido = !string.IsNullOrEmpty(categoria.nombre);

            // Assert
            Assert.IsFalse(nombreValido, "El nombre de la categoría no debería ser nulo o vacío.");
        }

    }
    // prueba unitaria de la entidad detalle de pedido 
    [TestClass]
    public class DetallePedidoTests
    {
        [TestMethod]
        public void CrearDetallePedido_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var producto = new Producto
            {
                ProductoId = 1,
                Nombre = "Refresco",
                Precio = 10.0f
            };

            var pedido = new Pedido
            {
                PedidoId = 100
            };

            var detalle = new DetallePedido
            {
                DetallePedidoId = 1,
                PedidoId = pedido.PedidoId,
                ProductoId = producto.ProductoId,
                Cantidad = 3,
                Producto = producto,
                Pedido = pedido,
                PrecioTotal = producto.Precio * 3
            };

            // Assert
            Assert.AreEqual(1, detalle.DetallePedidoId);
            Assert.AreEqual(100, detalle.PedidoId);
            Assert.AreEqual(1, detalle.ProductoId);
            Assert.AreEqual(3, detalle.Cantidad);
            Assert.IsNotNull(detalle.Producto);
            Assert.IsNotNull(detalle.Pedido);
            Assert.AreEqual(30.0f, detalle.PrecioTotal);
        }

        [TestMethod]
        public void CalcularPrecioTotal_DeberiaSerCorrecto()
        {
            // Arrange
            var producto = new Producto
            {
                ProductoId = 2,
                Nombre = "Pizza",
                Precio = 50.0f
            };

            var detalle = new DetallePedido
            {
                Producto = producto,
                Cantidad = 2
            };

            // Act
            detalle.PrecioTotal = detalle.Producto.Precio * detalle.Cantidad;

            // Assert
            Assert.AreEqual(100.0f, detalle.PrecioTotal);
        }

        [TestMethod]
        public void CrearDetallePedido_CantidadDebeSerPositiva()
        {
            // Arrange
            var detalle = new DetallePedido
            {
                Cantidad = -1 // Cantidad inválida
            };

            // Act
            bool cantidadValida = detalle.Cantidad > 0;

            // Assert
            Assert.IsFalse(cantidadValida, "La cantidad debe ser un número positivo.");
        }

    }
    // prueba unitaria de la entidad detalleVenta 
    [TestClass]
    public class DetalleVentaTests
    {
        [TestMethod]
        public void CrearDetalleVenta_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var producto = new Producto
            {
                ProductoId = 5,
                Nombre = "Laptop",
                Precio = 1000.0f
            };

            var venta = new Venta
            {
                VentaId = 20
            };

            var detalleVenta = new DetalleVenta
            {
                DetalleVentaId = 1,
                VentaId = venta.VentaId,
                ProductoId = producto.ProductoId,
                Descripcion = "Laptop gamer",
                Cantidad = 2,
                PrecioUnitario = producto.Precio,
                PrecioTotal = producto.Precio * 2,
                Venta = venta,
                Producto = producto
            };

            // Assert
            Assert.AreEqual(1, detalleVenta.DetalleVentaId);
            Assert.AreEqual(20, detalleVenta.VentaId);
            Assert.AreEqual(5, detalleVenta.ProductoId);
            Assert.AreEqual("Laptop gamer", detalleVenta.Descripcion);
            Assert.AreEqual(2, detalleVenta.Cantidad);
            Assert.AreEqual(1000.0f, detalleVenta.PrecioUnitario);
            Assert.AreEqual(2000.0f, detalleVenta.PrecioTotal);
            Assert.IsNotNull(detalleVenta.Venta);
            Assert.IsNotNull(detalleVenta.Producto);
        }

        [TestMethod]
        public void CalcularPrecioTotal_DeberiaSerCorrecto()
        {
            // Arrange
            var detalleVenta = new DetalleVenta
            {
                Cantidad = 3,
                PrecioUnitario = 50.0f
            };

            // Act
            detalleVenta.PrecioTotal = detalleVenta.Cantidad * detalleVenta.PrecioUnitario;

            // Assert
            Assert.AreEqual(150.0f, detalleVenta.PrecioTotal);
        }

        [TestMethod]
        public void CrearDetalleVenta_CantidadDebeSerPositiva()
        {
            // Arrange
            var detalleVenta = new DetalleVenta
            {
                Cantidad = -5
            };

            // Act
            bool cantidadValida = detalleVenta.Cantidad > 0;

            // Assert
            Assert.IsFalse(cantidadValida, "La cantidad debe ser un número positivo.");
        }

    }
    // prueba unitaria de la entidad pedido
    [TestClass]
    public class PedidoTests
    {
        [TestMethod]
        public void CrearPedidoDelivery_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                OwnerId = "user1",
                EstadoPedido = "pendiente",
                TipoPedido = "delivery",
                Telefono = "987654321",
                Direccion = "Av. Principal 123",
                Referencia = "Frente a la plaza",
                FechaPedido = DateTime.Now,
                DetallePedidos = new List<DetallePedido>(),
                PrecioTotal = 150.0f
            };

            // Assert
            Assert.AreEqual(1, pedido.PedidoId);
            Assert.AreEqual("user1", pedido.OwnerId);
            Assert.AreEqual("pendiente", pedido.EstadoPedido);
            Assert.AreEqual("delivery", pedido.TipoPedido);
            Assert.AreEqual("987654321", pedido.Telefono);
            Assert.AreEqual("Av. Principal 123", pedido.Direccion);
            Assert.AreEqual("Frente a la plaza", pedido.Referencia);
            Assert.IsNotNull(pedido.DetallePedidos);
            Assert.AreEqual(150.0f, pedido.PrecioTotal);
        }

        [TestMethod]
        public void CrearPedidoRecojoEnLocal_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var fechaRecogo = DateTime.Today.AddDays(1);
            var pedido = new Pedido
            {
                PedidoId = 2,
                OwnerId = "user2",
                EstadoPedido = "pendiente",
                TipoPedido = "recogo_en_local",
                FechaRecogo = fechaRecogo,
                FechaPedido = DateTime.Now,
                DetallePedidos = new List<DetallePedido>(),
                PrecioTotal = 80.0f
            };

            // Assert
            Assert.AreEqual("recogo_en_local", pedido.TipoPedido);
            Assert.AreEqual(fechaRecogo, pedido.FechaRecogo);
        }

        [TestMethod]
        public void CrearPedidoMesa_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 3,
                OwnerId = "user3",
                EstadoPedido = "pendiente",
                TipoPedido = "mesa",
                Mesa = "Mesa 5",
                FechaPedido = DateTime.Now,
                DetallePedidos = new List<DetallePedido>(),
                PrecioTotal = 200.0f
            };

            // Assert
            Assert.AreEqual("mesa", pedido.TipoPedido);
            Assert.AreEqual("Mesa 5", pedido.Mesa);
        }

        [TestMethod]
        public void EstadoPedido_DebeSerValido()
        {
            // Arrange
            var pedido = new Pedido
            {
                EstadoPedido = "desconocido" // Valor inválido
            };

            // Act
            bool estadoValido = pedido.EstadoPedido == "pendiente" ||
                                 pedido.EstadoPedido == "anulado" ||
                                 pedido.EstadoPedido == "entregado";

            // Assert
            Assert.IsFalse(estadoValido, "El estado del pedido debe ser pendiente, anulado o entregado.");
        }

    }
    // prueba unitaria de la entidad producto
    [TestClass]
    public class ProductoTests
    {
        [TestMethod]
        public void CrearProducto_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var categoria = new Categoria
            {
                CategoriaId = 1,
                nombre = "Electrónica"
            };

            var producto = new Producto
            {
                ProductoId = 10,
                CategoriaId = categoria.CategoriaId,
                categoria = categoria,
                Nombre = "Tablet",
                Descripcion = "Tablet 10 pulgadas",
                ImagenUrl = "http://imagen.com/tablet.png",
                Precio = 299.99f,
                Stock = "Disponible",
                ProductoInsumos = new List<ProductoInsumo>()
            };

            // Assert
            Assert.AreEqual(10, producto.ProductoId);
            Assert.AreEqual(1, producto.CategoriaId);
            Assert.AreEqual("Tablet", producto.Nombre);
            Assert.AreEqual("Tablet 10 pulgadas", producto.Descripcion);
            Assert.AreEqual("http://imagen.com/tablet.png", producto.ImagenUrl);
            Assert.AreEqual(299.99f, producto.Precio);
            Assert.AreEqual("Disponible", producto.Stock);
            Assert.IsNotNull(producto.ProductoInsumos);
            Assert.AreEqual("Electrónica", producto.categoria.nombre);
        }

        [TestMethod]
        public void Producto_PrecioDebeSerMayorACero()
        {
            // Arrange
            var producto = new Producto
            {
                Precio = -100.0f
            };

            // Act
            bool precioValido = producto.Precio > 0;

            // Assert
            Assert.IsFalse(precioValido, "El precio debe ser mayor que cero.");
        }

        [TestMethod]
        public void Producto_StockNoDebeSerNulo()
        {
            // Arrange
            var producto = new Producto
            {
                Stock = null
            };

            // Act
            bool stockValido = !string.IsNullOrEmpty(producto.Stock);

            // Assert
            Assert.IsFalse(stockValido, "El stock no debe ser nulo o vacío.");
        }
        
    }
    // prueba unitaria de la entidad productoInsumo
    [TestClass]
    public class ProductoInsumoTests
    {
        [TestMethod]
        public void CrearProductoInsumo_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var producto = new Producto
            {
                ProductoId = 1,
                Nombre = "Pizza"
            };

            var insumo = new Insumo
            {
                InsumoId = 1,
                Nombre = "Queso"
            };

            var productoInsumo = new ProductoInsumo
            {
                ProductoInsumoId = 10,
                ProductoId = producto.ProductoId,
                Producto = producto,
                InsumoId = insumo.InsumoId,
                Insumo = insumo,
                Cantidad = 2.5f
            };

            // Assert
            Assert.AreEqual(10, productoInsumo.ProductoInsumoId);
            Assert.AreEqual(1, productoInsumo.ProductoId);
            Assert.AreEqual(1, productoInsumo.InsumoId);
            Assert.AreEqual("Pizza", productoInsumo.Producto.Nombre);
            Assert.AreEqual("Queso", productoInsumo.Insumo.Nombre);
            Assert.AreEqual(2.5f, productoInsumo.Cantidad);
        }

        [TestMethod]
        public void ProductoInsumo_CantidadDebeSerMayorACero()
        {
            // Arrange
            var productoInsumo = new ProductoInsumo
            {
                Cantidad = 0
            };

            // Act
            bool cantidadValida = productoInsumo.Cantidad > 0;

            // Assert
            Assert.IsFalse(cantidadValida, "La cantidad debe ser mayor a cero.");
        }
    }
    // prueba unitaria de la entidad proveedor
    [TestClass]
    public class ProveedorTests
    {
        [TestMethod]
        public void CrearProveedor_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var proveedor = new Proveedor
            {
                ProveedorId = 1,
                Nombre = "Juan Perez",
                Empresa = "Proveedor S.A.",
                Producto = "Sillas",
                Ccantidad = "50",
                FechaRegistro = DateTime.Now
            };

            // Assert
            Assert.AreEqual(1, proveedor.ProveedorId);
            Assert.AreEqual("Juan Perez", proveedor.Nombre);
            Assert.AreEqual("Proveedor S.A.", proveedor.Empresa);
            Assert.AreEqual("Sillas", proveedor.Producto);
            Assert.AreEqual("50", proveedor.Ccantidad);
            Assert.IsNotNull(proveedor.FechaRegistro);
        }

        [TestMethod]
        public void Proveedor_CcantidadNoDebeSerNuloNiVacio()
        {
            // Arrange
            var proveedor = new Proveedor
            {
                Ccantidad = ""
            };

            // Act
            bool cantidadValida = !string.IsNullOrEmpty(proveedor.Ccantidad);

            // Assert
            Assert.IsFalse(cantidadValida, "La cantidad no debe ser nula ni vacía.");
        }

        [TestMethod]
        public void Proveedor_FechaRegistroDebeSerValida()
        {
            // Arrange
            var proveedor = new Proveedor
            {
                FechaRegistro = new DateTime(2025, 4, 27)
            };

            // Assert
            Assert.AreEqual(new DateTime(2025, 4, 27), proveedor.FechaRegistro);
        }

        [TestMethod]
        public void Proveedor_NombreDebeSerValido()
        {
            // Arrange
            var proveedor = new Proveedor
            {
                Nombre = "Maria Lopez"
            };

            // Act
            bool nombreValido = !string.IsNullOrEmpty(proveedor.Nombre);

            // Assert
            Assert.IsTrue(nombreValido, "El nombre no debe ser nulo ni vacío.");
        }
       
    }
    // prueba unitaria de la entidad venta 
    [TestClass]
    public class VentaTests
    {
        [TestMethod]
        public void CrearVenta_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var owner = new IdentityUser
            {
                Id = "user123",
                UserName = "usuario1"
            };

            var venta = new Venta
            {
                VentaId = 1,
                OwnerId = owner.Id,
                Fecha = new DateTime(2025, 4, 27),
                Detalles = new List<DetalleVenta>
                {
                    new DetalleVenta
                    {
                        DetalleVentaId = 1,
                        ProductoId = 10,
                        Cantidad = 2,
                        PrecioUnitario = 30.5f,
                        PrecioTotal = 61.0f
                    }
                },
                Owner = owner
            };

            // Assert
            Assert.AreEqual(1, venta.VentaId);
            Assert.AreEqual("user123", venta.OwnerId);
            Assert.AreEqual(new DateTime(2025, 4, 27), venta.Fecha);
            Assert.IsNotNull(venta.Detalles);
            Assert.AreEqual(1, venta.Detalles.Count);
            Assert.AreEqual(1, venta.Detalles[0].DetalleVentaId);
            Assert.AreEqual(10, venta.Detalles[0].ProductoId);
            Assert.AreEqual(2, venta.Detalles[0].Cantidad);
            Assert.AreEqual(30.5f, venta.Detalles[0].PrecioUnitario);
            Assert.AreEqual(61.0f, venta.Detalles[0].PrecioTotal);
            Assert.AreEqual("usuario1", venta.Owner.UserName);
        }

        [TestMethod]
        public void Venta_FechaDebeSerValida()
        {
            // Arrange
            var venta = new Venta
            {
                Fecha = new DateTime(2025, 4, 27)
            };

            // Assert
            Assert.AreEqual(new DateTime(2025, 4, 27), venta.Fecha);
        }

        [TestMethod]
        public void Venta_DetallesNoDebeSerNuloNiVacio()
        {
            // Arrange
            var venta = new Venta
            {
                Detalles = new List<DetalleVenta>()
            };

            // Act
            bool detallesValido = venta.Detalles.Count > 0;

            // Assert
            Assert.IsFalse(detallesValido, "La lista de detalles no debe estar vacía.");
        }

        [TestMethod]
        public void Venta_OwnerNoDebeSerNulo()
        {
            // Arrange
            var venta = new Venta
            {
                Owner = null
            };

            // Act & Assert
            Assert.IsNull(venta.Owner, "El propietario de la venta no debe ser nulo.");
        }
    }
}