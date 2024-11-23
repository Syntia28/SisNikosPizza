using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Utilidades
{
    public class VCG
    {
        public const string Satisfactorio = "Satisfactorio";
        public const string Errado = "Errado";

        // Roles del sistemas
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";
        public const string Role_Usuario = "Usuario";
        public const string Role_Manager = "Manager";

        public static class EstadoPedido
        {
            public const string Pendiente = "Pendiente";
            public const string Entregado = "Entregado";
            public const string Anulado = "Anulado";
            public const string Pagado = "Pagado";
        }

        public static class TipoPedido
        {
            public const string Delivery = "Delivery";
            public const string Recoger = "Recoger Local";
            public const string Mesa = "Mesa";
        }
    }

 
}
