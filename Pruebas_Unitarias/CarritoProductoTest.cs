using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;

namespace Pruebas_Unitarias
{
    [TestClass]
    public class CarritoProductoTests
    {
        [TestMethod]
        public void CrearCarritoProducto_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var producto = new Producto
            {
                ProductoId = 1,
                Nombre = "Pizza de Pepperoni",
                Precio = (float)25.0m
            };

            var carritoProducto = new CarritoProducto
            {
                Producto = producto,
                UserId = 123,
                Cantidad = 2
            };

            // Assert
            Assert.IsNotNull(carritoProducto.Producto);
            Assert.AreEqual(1, carritoProducto.Producto.ProductoId);
            Assert.AreEqual("Pizza de Pepperoni", carritoProducto.Producto.Nombre);
            Assert.AreEqual((float)25.0m, carritoProducto.Producto.Precio);
            Assert.AreEqual(123, carritoProducto.UserId);
            Assert.AreEqual(2, carritoProducto.Cantidad);
        }

        [TestMethod]
        public void CarritoProducto_CantidadMayorACero_Valido()
        {
            // Arrange
            var carritoProducto = new CarritoProducto
            {
                Cantidad = 3
            };

            // Act
            bool valido = carritoProducto.Cantidad > 0;

            // Assert
            Assert.IsTrue(valido);
        }

        [TestMethod]
        public void CarritoProducto_ProductoNulo_NoValido()
        {
            var carritoProducto = new CarritoProducto
            {
                Producto = null,
                Cantidad = 1
            };

            bool valido = carritoProducto.Producto != null;

            Assert.IsFalse(valido);
        }

        [TestMethod]
        public void CarritoProducto_CantidadNegativa_NoValido()
        {
            var carritoProducto = new CarritoProducto
            {
                Cantidad = -5
            };

            bool valido = carritoProducto.Cantidad > 0;

            Assert.IsFalse(valido);
        }
    }
}
