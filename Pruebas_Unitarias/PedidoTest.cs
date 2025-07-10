using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pruebas_Unitarias
{
    [TestClass]
    public class PedidoTests
    {
        [TestMethod]
        public void CrearPedido_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var user = new IdentityUser { Id = "user1", UserName = "cliente@test.com" };

            var pedido = new Pedido
            {
                PedidoId = 1,
                UserId = user.Id,
                User = user,
                EstadoPedido = "En preparación",
                TipoPedido = "Delivery",
                Pagado = true,
                Telefono = "999999999",
                Direccion = "Av. Perú 123",
                Referencia = "Frente al parque",
                FechaRecogo = DateTime.Now.AddHours(1),
                Mesa = null,
                FechaPedido = DateTime.Now,
                DetallePedido = new List<DetallePedido>
                {
                    new DetallePedido { DetallePedidoId = 1, PedidoId = 1, ProductoId = 2, Cantidad = 1, PrecioUnitario = 30, PrecioTotal = 30 }
                }
            };

            // Assert
            Assert.AreEqual(1, pedido.PedidoId);
            Assert.AreEqual("user1", pedido.UserId);
            Assert.AreEqual("cliente@test.com", pedido.User.UserName);
            Assert.AreEqual("En preparación", pedido.EstadoPedido);
            Assert.AreEqual("Delivery", pedido.TipoPedido);
            Assert.IsTrue(pedido.Pagado.HasValue && pedido.Pagado.Value);
            Assert.AreEqual("999999999", pedido.Telefono);
            Assert.AreEqual("Av. Perú 123", pedido.Direccion);
            Assert.AreEqual("Frente al parque", pedido.Referencia);
            Assert.IsNotNull(pedido.FechaRecogo);
            Assert.IsNull(pedido.Mesa);
            Assert.IsNotNull(pedido.DetallePedido);
            Assert.AreEqual(1, pedido.DetallePedido.Count());
        }

        [TestMethod]
        public void Pedido_FechaRecogoMayorAFechaPedido_Valido()
        {
            // Arrange
            var ahora = DateTime.Now;

            var pedido = new Pedido
            {
                FechaPedido = ahora,
                FechaRecogo = ahora.AddHours(2)
            };

            // Act
            bool valido = pedido.FechaRecogo > pedido.FechaPedido;

            // Assert
            Assert.IsTrue(valido);
        }

        [TestMethod]
        public void Pedido_SinDetalle_DeberiaSerNulo()
        {
            var pedido = new Pedido
            {
                PedidoId = 10,
                DetallePedido = null
            };

            Assert.IsNull(pedido.DetallePedido);
        }

        [TestMethod]
        public void Pedido_TipoYEstado_NoVacios()
        {
            var pedido = new Pedido
            {
                EstadoPedido = "Listo",
                TipoPedido = "Recojo en tienda"
            };

            bool valido = !string.IsNullOrWhiteSpace(pedido.EstadoPedido) && !string.IsNullOrWhiteSpace(pedido.TipoPedido);

            Assert.IsTrue(valido);
        }

        [TestMethod]
        public void Pedido_TelefonoOpcional_PuedeSerNulo()
        {
            var pedido = new Pedido
            {
                Telefono = null
            };

            Assert.IsNull(pedido.Telefono);
        }
    }
}
