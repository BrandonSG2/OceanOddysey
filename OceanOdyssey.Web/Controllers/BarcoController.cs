using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OceanOdyssey.Application.Services.Interfaces;

namespace OceanOdyssey.Web.Controllers
{
    public class BarcoController : Controller
    {
        private readonly IServiceBarco _serviceBarco;

        public BarcoController(IServiceBarco serviceBarco)
        {
            _serviceBarco = serviceBarco;
        }

        // GET: BarcoController
        public async Task<ActionResult> Index()
        {
           var collection = await _serviceBarco.ListAsync();
            foreach (var barco in collection)
            {
                
                barco.TotalHabitaciones = await _serviceBarco.GetTotalHabitaciones(barco.Id);
            }

            return View(collection);
        }

        // GET: BarcoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
           var @object = await _serviceBarco.FindByIdAsync(id);
           return View(@object);
        }

        // GET: BarcoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BarcoController/Create
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

        // GET: BarcoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BarcoController/Edit/5
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

        // GET: BarcoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BarcoController/Delete/5
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
