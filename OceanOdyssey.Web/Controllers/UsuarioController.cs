using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Implementations;
using OceanOdyssey.Application.Services.Interfaces;
using X.PagedList.Extensions;

namespace OceanOdyssey.Web.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServicePais _servicePais;
        public UsuarioController(IServiceUsuario serviceUsuario,IServicePais servicePais)
        {
            _serviceUsuario = serviceUsuario;
            _servicePais = servicePais;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceUsuario.ListAsync();
            return View(collection);
        }
        public async Task<IActionResult> Login(string id, string password)
        {
            var @object = await _serviceUsuario.LoginAsync(id, password);
            if (@object == null)
            {
                ViewBag.Message = "Error en Login o Password";
                return View("Login");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        public async Task<ActionResult> IndexAdmin(int? page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var collection = await _serviceUsuario.ListAsync();
            var listaPaginada = collection.ToList().ToPagedList(page ?? 1, 5);

            return View(listaPaginada);

        }
        [Authorize(Roles = "Admin")]
        // GET: UsuarioController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var @object = await _serviceUsuario.FindByIdAsync(id);
            return View(@object);
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> HistorialReservas(string id, int?page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            var collection = await _serviceUsuario.HistorialUsuario(id);
            var listaPaginada = collection.ToList().ToPagedList(page ?? 1, 5);

            return View(listaPaginada);
        }
        // GET: UsuarioController/Create
        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {
            ViewBag.ListPaises = await _servicePais.ListAsync();
            var habitaciones = await _servicePais.ListAsync();
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UsuarioDTO dto)
        {

            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                return BadRequest(errors);
            }

            await _serviceUsuario.AddAsync(dto);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
              "REGISTRO",
              "Registro Completo", Util.SweetAlertMessageType.success);
            
            return RedirectToAction("Index","Login");

        }
        [Authorize(Roles = "Admin")]
        // GET: UsuarioController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var @object = await _serviceUsuario.FindByIdAsync(id);
            return View(@object);
        }
        [Authorize(Roles = "Admin")]
        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UsuarioDTO dto)
        {
            await _serviceUsuario.UpdateAsync(id, dto);
            return RedirectToAction("Index");
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, IFormCollection collection)
        {
            await _serviceUsuario.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
