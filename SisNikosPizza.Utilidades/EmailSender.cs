using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SisNikosPizza.Utilidades
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            bool enviado = enviarCorreo.EnviarCorreo(email, subject, htmlMessage);

            if (!enviado)
            {
                Console.WriteLine("Error al enviar el correo");
                throw new Exception("No se pudo enviar el correo.");
            }

            return Task.CompletedTask;
        }
    }
}