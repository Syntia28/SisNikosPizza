using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;

namespace SisNikosPizza.Utilidades
{
    public class enviarCorreo
    {
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            string myEmail = "syntiaquiroz817@gmail.com";//tu correo
            string MyPassword = "ekpgyyzyineuckkp";//tu contraseña generada de aplicacion
            string MyAlias = "NikosPizza";//tu alias que deseas que aparezca cuando reciban tu correo
            MailMessage mCorreo;
            //creacion del cuerpo
            mCorreo = new MailMessage();
            mCorreo.From = new MailAddress(myEmail, MyAlias);
            mCorreo.To.Add(correo); //correo del destinatario
            mCorreo.Subject = asunto;//asunto
            mCorreo.Body = mensaje; //contenido del mensaje
            mCorreo.IsBodyHtml = true;
            mCorreo.Priority = MailPriority.High;
            //enviar correo
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(myEmail, MyPassword);
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.Send(mCorreo);
                //MessageBox.Show("Correo enviado correctamente.");
                return true; // Correo enviado con éxito
            }
            catch (Exception)
            {
                //MessageBox.Show("Error al enviar correo: " + ex.Message);
                return false; // Error al enviar el correo
            }
        }
    }
}