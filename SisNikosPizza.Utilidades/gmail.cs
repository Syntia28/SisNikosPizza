using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SisNikosPizza.Utilidades
{
    public class gmail
    {
        // Método para cargar y reemplazar los parámetros en el HTML
        public static string CargarYReemplazarHtml(string rutaArchivo, string userName)
        {
            // Leer el archivo HTML
            string htmlContent = File.ReadAllText(rutaArchivo);

            // Reemplazar valores dinámicos en el HTML
            htmlContent = htmlContent.Replace("{{USER_NAME}}", userName);

            return htmlContent;
        }
        public void CorreoDeCompra(string Correo, string user)
        {
            string basePath = Directory.GetCurrentDirectory();
            string path = Path.Combine(basePath + ".Utilidades", "views", "compra.html");

            string htmlContent = CargarYReemplazarHtml(path, user);

            string asunto = "CONFIRMACION DE PEDIDO";
            bool enviado = enviarCorreo.EnviarCorreo(Correo, asunto, htmlContent);
            if (!enviado)
            {
                Console.Write("Hubo un error. Verifique su conexión a internet o vuelva a intentarlo más tarde.");
            }
        }
    }
}