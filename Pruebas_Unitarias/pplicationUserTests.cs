using Microsoft.VisualStudio.TestTools.UnitTesting;
using SisNikosPizza.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pruebas_Unitarias
{
    [TestClass]
    public class ApplicationUserTests
    {
        [TestMethod]
        public void CrearUsuario_PropiedadesAsignadasCorrectamente()
        {
            // Arrange
            var user = new ApplicationUser
            {
                UserName = "correo@ejemplo.com",
                Email = "correo@ejemplo.com",
                Nombre = "Syntia Quiroz",
                Direccion = "Av. Lima 123",
                FechaNacimiento = new DateTime(1995, 5, 1),
                Role = "Administrador"
            };

            // Assert
            Assert.AreEqual("correo@ejemplo.com", user.UserName);
            Assert.AreEqual("correo@ejemplo.com", user.Email);
            Assert.AreEqual("Syntia Quiroz", user.Nombre);
            Assert.AreEqual("Av. Lima 123", user.Direccion);
            Assert.AreEqual(new DateTime(1995, 5, 1), user.FechaNacimiento);
            Assert.AreEqual("Administrador", user.Role);
        }

        [TestMethod]
        public void ValidarUsuario_SinNombre_DeberiaFallar()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Email = "usuario@prueba.com",
                // Nombre no asignado
                Direccion = "Sin nombre",
                Role = "Usuario"
            };

            // Act
            var resultado = ValidarModelo(user, out var errores);

            // Assert
            Assert.IsFalse(resultado);
            Assert.IsTrue(errores.Exists(e => e.ErrorMessage.Contains("Nombres es requerido")));
        }

        [TestMethod]
        public void ValidarUsuario_NombreMayor100Caracteres_DeberiaFallar()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Nombre = new string('A', 101), // 101 caracteres
                Email = "test@test.com"
            };

            // Act
            var resultado = ValidarModelo(user, out var errores);

            // Assert
            Assert.IsFalse(resultado);
            Assert.IsTrue(errores.Exists(e => e.ErrorMessage.Contains("maximum length")));
        }

        [TestMethod]
        public void ValidarUsuario_Correcto_DeberiaPasar()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Nombre = "Juan Pérez",
                Email = "juan@correo.com"
            };

            // Act
            var resultado = ValidarModelo(user, out var errores);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(0, errores.Count);
        }

        private bool ValidarModelo(object modelo, out List<ValidationResult> errores)
        {
            var contexto = new ValidationContext(modelo, null, null);
            errores = new List<ValidationResult>();
            return Validator.TryValidateObject(modelo, contexto, errores, validateAllProperties: true);
        }
    }
}
