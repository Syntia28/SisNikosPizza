using SisNikosPizza.Domain.Models;

namespace pruebasUnitarias
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            Producto p = new Producto();
            p.Nombre = "Jamon";
            p.Precio = 30;

            bool b = false;

            if (p.Nombre != null && p.Precio != 0)
            {
                b = true;
            }

            Assert.IsTrue(b);
        }
    }
}