using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SisNikosPizza.Controllers
{
    public class PedidodsController : Controller
    {
        // GET: PedidodsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PedidodsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PedidodsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PedidodsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PedidodsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PedidodsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PedidodsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PedidodsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
