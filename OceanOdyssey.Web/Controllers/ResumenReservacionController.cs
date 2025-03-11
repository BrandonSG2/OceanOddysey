using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OceanOdyssey.Application.Services.Interfaces;

namespace OceanOdyssey.Web.Controllers
{
    public class ResumenReservacionController : Controller
    {
        private readonly IServiceResumenReservacion _serviceResumenReservacion;

        public ResumenReservacionController(IServiceResumenReservacion serviceResumenReservacion)
        {
            _serviceResumenReservacion = serviceResumenReservacion;
        }

        // GET: ResumenReservacion
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceResumenReservacion.ListAsync();
         
            return View(collection);
        }

        // GET: ResumenReservacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                var @object = await _serviceResumenReservacion.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Reserva no existente");

                }

                return View(@object);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: ResumenReservacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResumenReservacion/Create
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

        // GET: ResumenReservacion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ResumenReservacion/Edit/5
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

        // GET: ResumenReservacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ResumenReservacion/Delete/5
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
