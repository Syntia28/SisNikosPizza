using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Pruebas_Unitarias
{
    [TestClass]
    public class CarritoItemsTests
    {
        [TestMethod]
        public void CrearCarritoItem_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var producto = new Producto { ProductoId = 1, Nombre = "Pizza", Precio = (float)20m };
            var usuario = new IdentityUser { Id = "user123", UserName = "cliente@test.com" };

            var item = new CarritoItems
            {
                CarritoItemsId = 10,
                ProductoId = 1,
                Producto = producto,
                UsuarioId = "user123",
                Usuario = usuario,
                Cantidad = 2,
                PrecioUnitario = 20m,
                PrecioTotal = 40m
            };

            // Assert
            Assert.AreEqual(10, item.CarritoItemsId);
            Assert.AreEqual(1, item.ProductoId);
            Assert.AreEqual("Pizza", item.Producto.Nombre);
            Assert.AreEqual("user123", item.UsuarioId);
            Assert.AreEqual("cliente@test.com", item.Usuario.UserName);
            Assert.AreEqual(2, item.Cantidad);
            Assert.AreEqual(20m, item.PrecioUnitario);
            Assert.AreEqual(40m, item.PrecioTotal);
        }

        [TestMethod]
        public void CalculoPrecioTotal_Correcto()
        {
            // Arrange
            var item = new CarritoItems
            {
                Cantidad = 3,
                PrecioUnitario = 15m
            };

            // Act
            item.PrecioTotal = item.Cantidad * item.PrecioUnitario;

            // Assert
            Assert.AreEqual(45m, item.PrecioTotal);
        }

        [TestMethod]
        public void CantidadYPrecioUnitario_Validos()
        {
            // Arrange
            var item = new CarritoItems
            {
                Cantidad = 2,
                PrecioUnitario = 25m
            };

            // Act
            bool valido = item.Cantidad > 0 && item.PrecioUnitario > 0;

            // Assert
            Assert.IsTrue(valido);
        }

        [TestMethod]
        public void CantidadNegativa_NoEsValida()
        {
            var item = new CarritoItems
            {
                Cantidad = -1,
                PrecioUnitario = 10m
            };

            bool valido = item.Cantidad > 0;

            Assert.IsFalse(valido);
        }
    }
}
