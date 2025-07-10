using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;
using System.Collections.Generic;

namespace Pruebas_Unitarias
{
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
                Nombre = "Juan Pérez",
                Empresa = "Distribuidora S.A.",
                Cantidad = "500 kg",
                Insumos = new List<Insumo>
                {
                    new Insumo { InsumoId = 1, Nombre = "Harina", Precio = 10f }
                }
            };

            // Assert
            Assert.AreEqual(1, proveedor.ProveedorId);
            Assert.AreEqual("Juan Pérez", proveedor.Nombre);
            Assert.AreEqual("Distribuidora S.A.", proveedor.Empresa);
            Assert.AreEqual("500 kg", proveedor.Cantidad);
            Assert.IsNotNull(proveedor.Insumos);
            Assert.AreEqual(1, ((List<Insumo>)proveedor.Insumos).Count);
        }

        [TestMethod]
        public void Proveedor_SinInsumos_ListaPuedeSerNula()
        {
            var proveedor = new Proveedor
            {
                ProveedorId = 2,
                Nombre = "Carlos Ríos",
                Empresa = "Alimentos Perú",
                Cantidad = "100 cajas",
                Insumos = null
            };

            Assert.IsNull(proveedor.Insumos);
        }

        [TestMethod]
        public void Proveedor_NombreYEmpresa_NoVacios()
        {
            var proveedor = new Proveedor
            {
                Nombre = "Lissette",
                Empresa = "Proveedores del Norte"
            };

            bool valido = !string.IsNullOrWhiteSpace(proveedor.Nombre) && !string.IsNullOrWhiteSpace(proveedor.Empresa);
            Assert.IsTrue(valido);
        }

        [TestMethod]
        public void Proveedor_CantidadTexto_SePuedeLeer()
        {
            var proveedor = new Proveedor
            {
                Cantidad = "200 litros"
            };

            Assert.AreEqual("200 litros", proveedor.Cantidad);
        }
    }
}
