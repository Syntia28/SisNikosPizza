using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;
using System;

namespace Pruebas_Unitarias
{
    [TestClass]
    public class DetallePedidoTests
    {
        [TestMethod]
        public void CrearDetallePedido_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var producto = new Producto { ProductoId = 1, Nombre = "Pizza Familiar", Precio = (float)40m };
            var pedido = new Pedido { PedidoId = 10, FechaPedido = DateTime.Now };

            var detalle = new DetallePedido
            {
                DetallePedidoId = 100,
                PedidoId = pedido.PedidoId,
                Pedido = pedido,
                ProductoId = producto.ProductoId,
                Producto = producto,
                Cantidad = 2,
                PrecioUnitario = 40m,
                PrecioTotal = 80m
            };

            // Assert
            Assert.AreEqual(100, detalle.DetallePedidoId);
            Assert.AreEqual(10, detalle.PedidoId);
            Assert.AreEqual(1, detalle.ProductoId);
            Assert.AreEqual("Pizza Familiar", detalle.Producto.Nombre);
            Assert.AreEqual(2, detalle.Cantidad);
            Assert.AreEqual(40m, detalle.PrecioUnitario);
            Assert.AreEqual(80m, detalle.PrecioTotal);
        }

        [TestMethod]
        public void CalculoPrecioTotal_EsCorrecto()
        {
            // Arrange
            var detalle = new DetallePedido
            {
                Cantidad = 3,
                PrecioUnitario = 25m
            };

            // Act
            detalle.PrecioTotal = detalle.Cantidad * detalle.PrecioUnitario;

            // Assert
            Assert.AreEqual(75m, detalle.PrecioTotal);
        }

        [TestMethod]
        public void DetallePedido_CantidadInvalida()
        {
            var detalle = new DetallePedido
            {
                Cantidad = -1,
                PrecioUnitario = 20m
            };

            bool esValido = detalle.Cantidad > 0;

            Assert.IsFalse(esValido);
        }

        [TestMethod]
        public void DetallePedido_ProductoNoNulo()
        {
            var detalle = new DetallePedido
            {
                Producto = new Producto { ProductoId = 1, Nombre = "Pizza", Precio = (float)30m }
            };

            Assert.IsNotNull(detalle.Producto);
        }

        [TestMethod]
        public void DetallePedido_SinProducto_DeberiaSerInvalido()
        {
            var detalle = new DetallePedido
            {
                Producto = null
            };

            Assert.IsNull(detalle.Producto);
        }
    }
}
