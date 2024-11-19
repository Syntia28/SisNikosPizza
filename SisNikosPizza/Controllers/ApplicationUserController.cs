//using System.Net;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using NikosPizza.Application.Queries.Login.UserLoginQuery;

//namespace NikosPizza.Api.Controllers
//{
//    [Route("Users")]
//    public class ApplicationUserController : Controller
//    {
//        private readonly IMediator _mediator;

//        public ApplicationUserController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        [HttpGet("Login")]
//        public IActionResult Login()
//        {
//            // Muestra el formulario de inicio de sesión
//            return View();
//        }

//        [HttpPost("Login")]
//        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
//        public async Task<IActionResult> Login([FromForm] UserLoginQuery request)
//        {
//            var result = await _mediator.Send(request);
//            if (result.IsSuccess)
//            {
//                return RedirectToAction("Welcome");
//            }
//            else
//            {
//                ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
//                return View();
//            }
//        }
//        [HttpGet("Welcome")]
//        public IActionResult Welcome()
//        {
//            return View();
//        }
//    }
//}
