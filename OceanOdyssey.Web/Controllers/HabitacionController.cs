﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Implementations;
using OceanOdyssey.Application.Services.Interfaces;
using X.PagedList.Extensions;
namespace OceanOdyssey.Web.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly IServiceHabitacion _serviceHabitacion;
        // GET: HabitacionController
        public HabitacionController(IServiceHabitacion serviceHabitacion)
        {
            _serviceHabitacion = serviceHabitacion;
        }
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<ActionResult> Index()
        {
          
           
            var collection=await _serviceHabitacion.ListAsync();
            return View(collection);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> IndexAdmin(int? page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var collection = await _serviceHabitacion.ListAsync();
            var listaPaginada = collection.ToList().ToPagedList(page ?? 1, 5);

            return View(listaPaginada);

        }
        // GET: HabitacionController/Details/5
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<ActionResult> Details(int id)
        {
            var @object= await _serviceHabitacion.FindByIdAsync(id);
            return View(@object);
        }

        // GET: HabitacionController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: HabitacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(HabitacionDTO habitacionDTO)
        {
                
                var habi = await _serviceHabitacion.FindByIdAsync(habitacionDTO.Id);
                var habitaciones= await _serviceHabitacion.ListAsync();
           
            var existeNombre = await _serviceHabitacion.ExisteNombreAsync(habitacionDTO.Nombre);
            if (existeNombre)
            {
                ModelState.AddModelError("Nombre", "El nombre de la habitación ya está registrado. Elige otro.");
                return View(habitacionDTO); 
            }

            if (habi != null)
                {
                TempData["Mensaje"] = "El barco ya existe.";
                return RedirectToAction("IndexAdmin");
                  }
                
                
                await _serviceHabitacion.AddAsync(habitacionDTO);
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
               "Crear Habitacion",
               "Habitacion creada", Util.SweetAlertMessageType.success);
            return RedirectToAction("IndexAdmin");

        }
        [HttpGet]
        public async Task<IActionResult> ValidarNombre(string nombre)
        {
            var existe = await _serviceHabitacion.ExisteNombreAsync(nombre);
            return Json(existe);
        }
        [HttpGet]
        public async Task<IActionResult> ValidarNombreAct(string nombre, int id)
        {
            var existe = await _serviceHabitacion.ExisteNombreActAsync(nombre,id);
            return Json(existe); 
        }
        // GET: HabitacionController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {

            var @object = await _serviceHabitacion.FindByIdAsync(id);
            return View(@object);
        }
        // POST: HabitacionController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HabitacionDTO dto)
        { 

            var existeNombre = await _serviceHabitacion.ExisteNombreActAsync(dto.Nombre, id);
            if (existeNombre)
            {
                ModelState.AddModelError("Nombre", "El nombre de la habitación ya está registrado. Elige otro.");
                return View(dto);
            }
            await _serviceHabitacion.UpdateAsync(id, dto);
                TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
               "Habitacion actualizada",
               "Habitacion Actualizada", Util.SweetAlertMessageType.success);
                return RedirectToAction("IndexAdmin");
            


        }

        // POST: HabitacionController/Delete/5
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
