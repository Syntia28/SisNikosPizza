using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;
using System.Collections.Generic;

namespace Pruebas_Unitarias
{
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
                nombre = "Bebidas",
                producto = new List<Producto>
                {
                    new Producto { ProductoId = 1, Nombre = "Coca Cola", Precio = (float) 5m },
                    new Producto { ProductoId = 2, Nombre = "Inka Cola", Precio = (float) 4.5m }
                }
            };

            // Assert
            Assert.AreEqual(1, categoria.CategoriaId);
            Assert.AreEqual("Bebidas", categoria.nombre);
            Assert.IsNotNull(categoria.producto);
            Assert.AreEqual(2, ((List<Producto>)categoria.producto).Count);
        }

        [TestMethod]
        public void Categoria_SinProductos_ProductoEsNull()
        {
            // Arrange
            var categoria = new Categoria
            {
                CategoriaId = 2,
                nombre = "Snacks",
                producto = null
            };

            // Assert
            Assert.IsNull(categoria.producto);
        }

        [TestMethod]
        public void Categoria_SinNombre_NoEsValida()
        {
            // Arrange
            var categoria = new Categoria
            {
                CategoriaId = 3,
                nombre = ""
            };

            // Act
            bool esValido = !string.IsNullOrWhiteSpace(categoria.nombre);

            // Assert
            Assert.IsFalse(esValido);
        }

        [TestMethod]
        public void Categoria_ConProductos_VerificaContenido()
        {
            // Arrange
            var productos = new List<Producto>
            {
                new Producto { ProductoId = 1, Nombre = "Sprite", Precio = (float) 4m }
            };

            var categoria = new Categoria
            {
                CategoriaId = 4,
                nombre = "Gaseosas",
                producto = productos
            };

            // Assert
            Assert.IsTrue(categoria.producto.Any(p => p.Nombre == "Sprite"));
        }
    }
}
