using Humanizer;
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
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceCrucero.ListAsync();
            return View(collection);
        }

        // GET: CruceroController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceCrucero.FindByIdAsync(id);
            Console.Write(@object);
            return View(@object);
        }

        // GET: CruceroController/Create
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
        public async Task<IActionResult> Create(CruceroDTO cruceroDTO, IFormFile imageFile)
        {
            MemoryStream target = new MemoryStream();

            // Buscar el barco correspondiente
            var barco = await _serviceBarco.FindByIdAsync(cruceroDTO.Idbarco);

            // Verificar si el barco existe
            if (barco == null)
            {
                TempData.Keep();
                return BadRequest("Barco no existe");
            }

         

            // Si no se proporcionó una imagen, pero se subió un archivo de imagen
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

            // Establecer la relación entre el barco y el crucero
            cruceroDTO.IdbarcoNavigation = barco;

            // Deserializar los itinerarios desde TempData si están disponibles
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

            // Recuperar las fechas desde TempData
            if (TempData["ListaFechas"] != null)
            {
                string jsonFechas = (string)TempData["ListaFechas"];
                var fechas = JsonSerializer.Deserialize<List<FechaCruceroDTO>>(jsonFechas);

                if (fechas != null && fechas.Any())
                {
                   
                    cruceroDTO.FechaCrucero = fechas;
                }
            }

            // Llamar al servicio para agregar el crucero junto con los itinerarios y las fechas asociadas
            await _serviceCrucero.AddAsync(cruceroDTO);

            // Redirigir a la página de listado o índice
            return RedirectToAction("Index");
        }





        // GET: CruceroController/Edit/5
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

            // Verifica si "ItinerarioList" existe en TempData
            if (TempData["ItinerarioList"] != null)
            {
                json = (string)TempData["ItinerarioList"]!;
                listaItinerarios = JsonSerializer.Deserialize<List<ItinerarioDTO>>(json)!;
            }
            else
            {
                // Si no existe, inicializa la lista vacía
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

            // Solo cargar TempData si hay datos previos
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
                // Si la fecha ya existe, evitar duplicados
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

         

            // Guardar la lista en TempData
            TempData["ListaFechas"] = JsonSerializer.Serialize(listaFechas);
            TempData.Keep();
            await Task.CompletedTask;
            return Ok();
        }







    }
}
