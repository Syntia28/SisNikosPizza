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
            string myEmail = "syntiaquiroz817@gmail.com";
            string MyPassword = "ekpgyyzyineuckkp";
            string MyAlias = "NikosPizza";
            MailMessage mCorreo;
            mCorreo = new MailMessage();
            mCorreo.From = new MailAddress(myEmail, MyAlias);
            mCorreo.To.Add(correo); 
            mCorreo.Subject = asunto;
            mCorreo.Body = mensaje; 
            mCorreo.IsBodyHtml = true;
            mCorreo.Priority = MailPriority.High;
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
                return true; 
            }
            catch (Exception)
            {
                return false; 
            }
        }
    }
}