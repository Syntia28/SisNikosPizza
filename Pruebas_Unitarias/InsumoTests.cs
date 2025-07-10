using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;
using System.Collections.Generic;

namespace Pruebas_Unitarias
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
                Precio = 15.5f,
                UnidadeDeMedida = "Kilogramos",
                Cantidad = 20f,
                ProductoInsumos = new List<ProductoInsumo>()
            };

            // Assert
            Assert.AreEqual(1, insumo.InsumoId);
            Assert.AreEqual("Harina", insumo.Nombre);
            Assert.AreEqual("Harina de trigo", insumo.Descripcion);
            Assert.AreEqual(15.5f, insumo.Precio);
            Assert.AreEqual("Kilogramos", insumo.UnidadeDeMedida);
            Assert.AreEqual(20f, insumo.Cantidad);
            Assert.IsNotNull(insumo.ProductoInsumos);
        }

        [TestMethod]
        public void CrearInsumo_SinRelacionConProductos_ListaVacia()
        {
            // Arrange
            var insumo = new Insumo
            {
                Nombre = "Sal",
                Descripcion = "Sal de mesa",
                Precio = 5f,
                UnidadeDeMedida = "Gramos",
                Cantidad = 1000f,
                ProductoInsumos = null 
            };

            // Assert
            Assert.IsNull(insumo.ProductoInsumos);
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
}
