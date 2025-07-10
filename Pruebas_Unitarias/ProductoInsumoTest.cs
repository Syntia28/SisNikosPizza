using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;

namespace Pruebas_Unitarias
{
    [TestClass]
    public class ProductoInsumoTests
    {
        [TestMethod]
        public void CrearProductoInsumo_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var producto = new Producto { ProductoId = 1, Nombre = "Pizza Americana", Precio = 25f };
            var insumo = new Insumo { InsumoId = 10, Nombre = "Queso", Precio = 8f };

            var productoInsumo = new ProductoInsumo
            {
                ProductoInsumoId = 100,
                ProductoId = producto.ProductoId,
                Producto = producto,
                InsumoId = insumo.InsumoId,
                Insumo = insumo,
                Cantidad = 0.5f
            };

            // Assert
            Assert.AreEqual(100, productoInsumo.ProductoInsumoId);
            Assert.AreEqual(1, productoInsumo.ProductoId);
            Assert.AreEqual("Pizza Americana", productoInsumo.Producto.Nombre);
            Assert.AreEqual(10, productoInsumo.InsumoId);
            Assert.AreEqual("Queso", productoInsumo.Insumo.Nombre);
            Assert.AreEqual(0.5f, productoInsumo.Cantidad);
        }

        [TestMethod]
        public void ProductoInsumo_CantidadValida()
        {
            // Arrange
            var productoInsumo = new ProductoInsumo
            {
                Cantidad = 1.2f
            };

            // Act
            bool esValido = productoInsumo.Cantidad > 0;

            // Assert
            Assert.IsTrue(esValido);
        }

        [TestMethod]
        public void ProductoInsumo_CantidadNegativa_NoValida()
        {
            var productoInsumo = new ProductoInsumo
            {
                Cantidad = -0.5f
            };

            bool esValido = productoInsumo.Cantidad > 0;

            Assert.IsFalse(esValido);
        }

        [TestMethod]
        public void ProductoInsumo_SinProductoYInsumo_DeberiaSerNulo()
        {
            var productoInsumo = new ProductoInsumo
            {
                Producto = null,
                Insumo = null
            };

            Assert.IsNull(productoInsumo.Producto);
            Assert.IsNull(productoInsumo.Insumo);
        }
    }
}
