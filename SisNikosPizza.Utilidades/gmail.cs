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
        public static string CargarYReemplazarHtml(string rutaArchivo, string userName)
        {
            string htmlContent = File.ReadAllText(rutaArchivo);

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
        }
    }
}