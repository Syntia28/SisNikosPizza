// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace SisNikosPizza.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El correo es obligatorio")]
            [EmailAddress(ErrorMessage = "Formato de correo inválido")]
            public string Email { get; set; }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("==============================================");
            Console.WriteLine("========= ERRORES DE VALIDACIÓN =========");
            Console.WriteLine($"recuperacion de cuenta iniciado: {ModelState.IsValid}");
            Console.WriteLine("==============================================");
            Console.WriteLine("==============================================");
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Email: {Input.Email}");
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null /*|| !(await _userManager.IsEmailConfirmedAsync(user))*/)
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code, email = user.Email },
                    protocol: Request.Scheme);

                string mensaje = $"Hola {user.UserName},\n\n" +
                     $"Has solicitado restablecer tu contraseña.\n" +
                     $"Haz clic en el siguiente enlace para hacerlo:\n\n" +
                     $"{callbackUrl}\n\n" +
                     $"Si no solicitaste esto, ignora este mensaje.";

                Console.WriteLine("==============================================");
                Console.WriteLine("==============================================");
                Console.WriteLine($"correo del usuario: {user.Email}");
                Console.WriteLine("==============================================");
                Console.WriteLine("==============================================");

                await _emailSender.SendEmailAsync(user.Email, "Restablecer Contraseña", mensaje);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }


            return Page();
        }
   
    }
}
