using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;
using System.Collections.Generic;

namespace Pruebas_Unitarias
{
    [TestClass]
    public class ProductoTests
    {
        [TestMethod]
        public void CrearProducto_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var categoria = new Categoria { CategoriaId = 1, nombre = "Pizzas" };

            var producto = new Producto
            {
                ProductoId = 10,
                CategoriaId = 1,
                categoria = categoria,
                Nombre = "Pizza Hawaiana",
                Descripcion = "Pizza con piña y jamón",
                ImagenUrl = "/imagenes/hawaiana.jpg",
                Precio = 30.5f,
                Stock = 15,
                ProductoInsumos = new List<ProductoInsumo>(),
                DetallePedidos = new List<DetallePedido>()
            };

            // Assert
            Assert.AreEqual(10, producto.ProductoId);
            Assert.AreEqual(1, producto.CategoriaId);
            Assert.AreEqual("Pizzas", producto.categoria.nombre);
            Assert.AreEqual("Pizza Hawaiana", producto.Nombre);
            Assert.AreEqual("Pizza con piña y jamón", producto.Descripcion);
            Assert.AreEqual("/imagenes/hawaiana.jpg", producto.ImagenUrl);
            Assert.AreEqual(30.5f, producto.Precio);
            Assert.AreEqual(15, producto.Stock);
            Assert.IsNotNull(producto.ProductoInsumos);
            Assert.IsNotNull(producto.DetallePedidos);
        }

        [TestMethod]
        public void Producto_PrecioValido_EsMayorACero()
        {
            var producto = new Producto
            {
                Precio = 10f
            };

            bool esValido = producto.Precio > 0;
            Assert.IsTrue(esValido);
        }

        [TestMethod]
        public void Producto_StockNoNegativo()
        {
            var producto = new Producto
            {
                Stock = 5
            };

            Assert.IsTrue(producto.Stock >= 0);
        }

        [TestMethod]
        public void Producto_SinInsumos_ListaPuedeSerNula()
        {
            var producto = new Producto
            {
                Nombre = "Pizza Margarita",
                ProductoInsumos = null
            };

            Assert.IsNull(producto.ProductoInsumos);
        }


        [TestMethod]
        public void Producto_SinImagenUrl_EsOpcional()
        {
            var producto = new Producto
            {
                Nombre = "Pizza Vegetariana",
                ImagenUrl = null
            };

            Assert.IsNull(producto.ImagenUrl);
        }
    }
}
