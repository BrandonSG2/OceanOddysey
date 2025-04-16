using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Implementations;
using OceanOdyssey.Application.Services.Interfaces;
using X.PagedList.Extensions;

namespace OceanOdyssey.Web.Controllers
{
    public class ComplementoController : Controller
    {
        private readonly IServiceComplemento _serviceComplemento;
        // GET: HabitacionController
        public ComplementoController(IServiceComplemento serviceComplemento)
        {
            _serviceComplemento = serviceComplemento;
        }
        // GET: ComplementoController
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceComplemento.ListAsync();
            return View(collection);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> IndexAdmin(int? page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var collection = await _serviceComplemento.ListAsync();
            var listaPaginada = collection.ToList().ToPagedList(page ?? 1, 5);

            return View(listaPaginada);

        }
        // GET: ComplementoController/Details/5
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceComplemento.FindByIdAsync(id);
            return View(@object);
        }

        // GET: ComplementoController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ComplementoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(ComplementoDTO complementoDTO)
        {

            var habi = await _serviceComplemento.FindByIdAsync(complementoDTO.Id);
            var habitaciones = await _serviceComplemento.ListAsync();

            var existeNombre = await _serviceComplemento.ExisteNombreAsync(complementoDTO.Nombre);
            if (existeNombre)
            {
                ModelState.AddModelError("Nombre", "El nombre de la habitación ya está registrado. Elige otro.");
                return View(complementoDTO);
            }

            if (habi != null)
            {
                TempData["Mensaje"] = "El barco ya existe.";
                return RedirectToAction("IndexAdmin");
            }


            await _serviceComplemento.AddAsync(complementoDTO);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
               "Crear Complemento",
               "Complemento creado", Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");
        }
        public async Task<IActionResult> ValidarNombre(string nombre)
        {
            var existe = await _serviceComplemento.ExisteNombreAsync(nombre);
            return Json(existe);
        }
        [HttpGet]
        public async Task<IActionResult> ValidarNombreAct(string nombre, int id)
        {
            var existe = await _serviceComplemento.ExisteNombreActAsync(nombre, id);
            return Json(existe);
        }
        // GET: ComplementoController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id)
        {
            var @object = await _serviceComplemento.FindByIdAsync(id);
            return View(@object);
        }
        [Authorize(Roles = "Admin")]
        // POST: ComplementoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComplementoDTO dto)
        {

            var existeNombre = await _serviceComplemento.ExisteNombreActAsync(dto.Nombre, id);
            if (existeNombre)
            {
                ModelState.AddModelError("Nombre", "El nombre de la habitación ya está registrado. Elige otro.");
                return View(dto);
            }
            await _serviceComplemento.UpdateAsync(id, dto);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
           "Complemento actualizado",
           "Complemento Actualizado", Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");



        }

        // GET: ComplementoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ComplementoController/Delete/5
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
