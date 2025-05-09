﻿using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Implementations;
using OceanOdyssey.Application.Services.Interfaces;
using System.Text.Json;

namespace OceanOdyssey.Web.Controllers
{
    public class CruceroController : Controller
    {
        private readonly IServiceCrucero _serviceCrucero;
        private readonly IServiceBarco _serviceBarco;
        private readonly IServiceHabitacion _serviceHabitacion;
        private readonly IServicePuerto _servicePuerto;

        public CruceroController(IServiceCrucero serviceCrucero, IServiceBarco serviceBarco, IServiceHabitacion serviceHabitacion, IServicePuerto servicePuerto)
        {
            _serviceCrucero = serviceCrucero;
            _serviceBarco = serviceBarco;
            _serviceHabitacion = serviceHabitacion;
            _servicePuerto = servicePuerto;
        }






        // GET: HabitacionController

        // GET: CruceroController
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceCrucero.ListAsync();
            return View(collection);
        }

        // GET: CruceroController/Details/5
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceCrucero.FindByIdAsync(id);
            Console.Write(@object);
            return View(@object);
        }

        // GET: CruceroController/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {


            ViewBag.ListBarco = await _serviceBarco.ListAsync();
            ViewBag.ListHabitaciones = await _serviceHabitacion.ListAsync();
            ViewBag.ListPuertos = await _servicePuerto.ListAsync();


            return View();
        }



        [HttpGet]
        public async Task<IActionResult> GetHabitacionesPorBarco(int idBarco)
        {
            var habitaciones = await _serviceHabitacion.ObtenerHabitacionesPorBarcoAsync(idBarco);

            var resultado = habitaciones.Select(h => new { id = h.Id, nombre = h.Nombre }).ToList();

            return Json(resultado);
        }


        // POST: CruceroController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CruceroDTO cruceroDTO, IFormFile imageFile)
        {
            MemoryStream target = new MemoryStream();

          
            var barco = await _serviceBarco.FindByIdAsync(cruceroDTO.Idbarco);

          
            if (barco == null)
            {
                TempData.Keep();
                return BadRequest("Barco no existe");
            }




          
            if (cruceroDTO.Imagen == null)
            {
                if (imageFile != null)
                {
                    imageFile.OpenReadStream().CopyTo(target);
                    cruceroDTO.Imagen = target.ToArray();
                    cruceroDTO.Disponible = true;
                    ModelState.Remove("Imagen");
                }
            }

         
            cruceroDTO.IdbarcoNavigation = barco;

          
            if (TempData["ItinerarioList"] != null)
            {
                string json = (string)TempData["ItinerarioList"];
                var itinerarios = JsonSerializer.Deserialize<List<ItinerarioDTO>>(json);

                if (itinerarios != null && itinerarios.Any())
                {

                    cruceroDTO.Itinerario = itinerarios;

                    itinerarios.ForEach(i => i.Idcrucero = cruceroDTO.Id);
                }
            }

         
            if (TempData["ListaFechas"] != null)
            {
                string jsonFechas = (string)TempData["ListaFechas"];
                var fechas = JsonSerializer.Deserialize<List<FechaCruceroDTO>>(jsonFechas);

                if (fechas != null && fechas.Any())
                {

                    cruceroDTO.FechaCrucero = fechas;
                }
            }

           
            await _serviceCrucero.AddAsync(cruceroDTO);

       
            return RedirectToAction("Index");
        }





        // GET: CruceroController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CruceroController/Edit/5
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

        // GET: CruceroController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CruceroController/Delete/5
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



        public async Task<IActionResult> AddItinerario(List<ItinerarioDTO> itinerarios)
        {
            var listaItinerarios = new List<ItinerarioDTO>();
            string json = "";

          
            if (TempData["ItinerarioList"] != null)
            {
                json = (string)TempData["ItinerarioList"]!;
                listaItinerarios = JsonSerializer.Deserialize<List<ItinerarioDTO>>(json)!;
            }
            else
            {
              
                listaItinerarios = new List<ItinerarioDTO>();
            }


            foreach (var itinerario in itinerarios)
            {

                var existingItinerario = listaItinerarios.FirstOrDefault(i => i.Dia == itinerario.Dia);

                if (existingItinerario != null)
                {

                    existingItinerario.Descripcion = itinerario.Descripcion;
                    existingItinerario.Idpuerto = itinerario.Idpuerto;
                }
                else
                {

                    listaItinerarios.Add(itinerario);
                }
            }


            json = JsonSerializer.Serialize(listaItinerarios);

            TempData["ItinerarioList"] = json;


            TempData.Keep();

            await Task.CompletedTask;

            return Ok();
        }


        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> AddFechas(List<FechaCruceroDTO> fechas)
        {
            List<FechaCruceroDTO> listaFechas = new List<FechaCruceroDTO>();

           
            if (TempData["ListaFechas"] != null)
            {
                string json = (string)TempData["ListaFechas"]!;
                if (!string.IsNullOrEmpty(json))
                {
                    listaFechas = JsonSerializer.Deserialize<List<FechaCruceroDTO>>(json) ?? new List<FechaCruceroDTO>();
                }
            }



            foreach (var fechaDto in fechas)
            {
             
                if (!listaFechas.Any(f => f.FechaInicio == fechaDto.FechaInicio))
                {
                    listaFechas.Add(new FechaCruceroDTO
                    {
                        FechaInicio = fechaDto.FechaInicio,
                        //FechaLimite = fechaDto.FechaLimite,
                        PrecioHabitacion = fechaDto.PrecioHabitacion ?? new List<PrecioHabitacionDTO>()
                    });
                }
            }



            TempData["ListaFechas"] = JsonSerializer.Serialize(listaFechas);
            TempData.Keep();
            await Task.CompletedTask;
            return Ok();
        }







    }
}
