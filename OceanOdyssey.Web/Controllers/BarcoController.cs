using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Interfaces;
using X.PagedList.Extensions;
using System.Text.Json;
using OceanOdyssey.Infraestructure.Models;
using System.Text.Json.Serialization;
using OceanOdyssey.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
namespace OceanOdyssey.Web.Controllers
{
    public class BarcoController : Controller
    {
        private readonly IServiceBarco _serviceBarco;
        private readonly IServiceHabitacion _serviceHabitacion;
        private readonly IServiceBarcoHabitacion _serviceBarcoHabitacion;
        public BarcoController(IServiceBarco serviceBarco,IServiceHabitacion serviceHabitacion, IServiceBarcoHabitacion serviceBarcoHabitacion)
        {
            _serviceBarco = serviceBarco;
            _serviceHabitacion = serviceHabitacion;
            _serviceBarcoHabitacion = serviceBarcoHabitacion;
        }

        // GET: BarcoController
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceBarco.ListAsync();
            foreach (var barco in collection)
            {

                barco.TotalHabitaciones = await _serviceBarco.GetTotalHabitaciones(barco.id);
            }

            return View(collection);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> IndexAdmin(int? page)
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            var collection = await _serviceBarco.ListAsync();
            foreach (var barco in collection)
            {

                barco.TotalHabitaciones = await _serviceBarco.GetTotalHabitaciones(barco.id);
            }
            var listaPaginada = collection.ToList().ToPagedList(page ?? 1, 5);

            return View(listaPaginada);

        }
        [Authorize(Roles = "Cliente,Admin")]
        // GET: BarcoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceBarco.FindByIdAsync(id);
            return View(@object);
        }

        // GET: BarcoController/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.ListHabitaciones = await _serviceHabitacion.ListAsync();
            var habitaciones = await _serviceHabitacion.ListAsync();
            TempData["BarcoCreate"] = null;
            TempData.Keep();

            return View();
        }
        
        public IActionResult DeleteLibro(int idHabitacion)
        {
            List<BarcoHabitacionDTO> lista = new List<BarcoHabitacionDTO>();
            string json = "";

            if (TempData["BarcoCreate"] != null)
            {
                json = (string)TempData["BarcoCreate"]!;
                lista = JsonSerializer.Deserialize<List<BarcoHabitacionDTO>>(json!)!;

              
                var habitacion = lista.FirstOrDefault(h => h.Idhabitacion == idHabitacion);
                if (habitacion != null)
                {
                    if (habitacion.Cantidad > 1)
                    {
                       
                        habitacion.Cantidad--;
                    }
                    else
                    {
                       
                        lista.Remove(habitacion);
                    }

               
                    json = JsonSerializer.Serialize(lista);
                    TempData["BarcoCreate"] = json;
                }
            }

            TempData.Keep("BarcoCreate");

            return PartialView("_DetailHabitaciones", lista);
        }
        
        public async Task<IActionResult> AddLibro(int id, int cantidad)
        {
            BarcoHabitacionDTO detalle = new();
            List<BarcoHabitacionDTO> lista = new();
            string json = "";

            var habitacion = await _serviceHabitacion.FindByIdAsync(id);
            if (habitacion == null)
            {
                return BadRequest("Habitación no encontrada.");
            }

            detalle.Cantidad = cantidad;
            detalle.Idhabitacion = habitacion.Id;
            detalle.IdhabitacionNavigation = habitacion;

           
            int? idBarco = TempData["IdBarcoEditando"] as int?;
            if (idBarco != null)
            {
                detalle.Idbarco = idBarco; 
            }

            if (TempData["BarcoCreate"] != null)
            {
                json = (string)TempData["BarcoCreate"]!;
                lista = JsonSerializer.Deserialize<List<BarcoHabitacionDTO>>(json!)!;

                var item = lista.FirstOrDefault(o => o.Idhabitacion == id);
                if (item != null)
                {
                    item.Cantidad += cantidad;
                }
                else
                {
                    lista.Add(detalle);
                }
            }
            else
            {
                lista.Add(detalle);
            }

            json = JsonSerializer.Serialize(lista);
            TempData["BarcoCreate"] = json;
            TempData.Keep("BarcoCreate");

            return PartialView("_DetailHabitaciones", lista);
        }
        // POST: BarcoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(BarcoDTO barcoDTO)
        {
            Console.WriteLine(JsonSerializer.Serialize(barcoDTO));
            if (!ModelState.IsValid)
            {
                return View(barcoDTO);
            }

            
            var existeNombre = await _serviceBarco.ExisteNombreAsync(barcoDTO.Nombre);
            if (existeNombre)
            {
                ModelState.AddModelError("Nombre", "El nombre del barco ya está registrado. Elige otro.");
                return View(barcoDTO);
            }

            
            List<BarcoHabitacionDTO> habitaciones = new();
            if (TempData["BarcoCreate"] != null)
            {
                string json = (string)TempData["BarcoCreate"]!;
                habitaciones = JsonSerializer.Deserialize<List<BarcoHabitacionDTO>>(json!) ?? new List<BarcoHabitacionDTO>();
            }
            if (!habitaciones.Any())
            {
                ModelState.AddModelError("BarcoHabitacion", "Debe seleccionar al menos una habitación.");
                return View(barcoDTO);
            }
         
            if (habitaciones.Any())
            {
                barcoDTO.BarcoHabitacion = habitaciones;

               
                habitaciones.ForEach(h => h.Idbarco = barcoDTO.id);
            }

         
            var barcoId = await _serviceBarco.AddAsync(barcoDTO);
            if (barcoId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Hubo un error al guardar el barco.");
                return View(barcoDTO);
            }

            // Mensaje de éxito
            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Barco Creado",
                "El barco ha sido registrado exitosamente.",
                Util.SweetAlertMessageType.success
            );

            return RedirectToAction("IndexAdmin"); 
        }

        // GET: BarcoController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var barco = await _serviceBarco.FindByIdAsync(id);
            TempData["IdBarcoEditando"] = id;
            if (barco == null)
            {
                return NotFound();
            }

            var barcoDTO = new BarcoDTO
            {
                id = barco.id,
                Nombre = barco.Nombre,
                Capacidad = barco.Capacidad,
                Descripcion = barco.Descripcion,
                BarcoHabitacion = barco.BarcoHabitacion.Select(h => new BarcoHabitacionDTO
                {
                    Idhabitacion = h.Idhabitacion,
                    Cantidad = h.Cantidad,
                    IdhabitacionNavigation = new HabitacionDTO
                    {
                        Id = h.IdhabitacionNavigation.Id,
                        Nombre = h.IdhabitacionNavigation.Nombre
                    }
                }).ToList()
            };

           
            if (TempData["BarcoCreate"] == null)
            {
                string json = JsonSerializer.Serialize(barcoDTO.BarcoHabitacion);
                TempData["BarcoCreate"] = json;
            }

            TempData.Keep("BarcoCreate"); 

            return View(barcoDTO);



        }

        // POST: BarcoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, BarcoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var existeNombre = await _serviceBarco.ExisteNombreActAsync(dto.Nombre, id);
            if (existeNombre)
            {
                ModelState.AddModelError("Nombre", "El nombre del barco ya está registrado. Elige otro.");
                return View(dto);
            }

            if (TempData["BarcoCreate"] != null)
            {
                var json = (string)TempData["BarcoCreate"];
                var habitacionesPrevias = JsonSerializer.Deserialize<List<BarcoHabitacionDTO>>(json);

                if (habitacionesPrevias != null)
                {
                    foreach (var habitacion in habitacionesPrevias)
                    {
                        if (!dto.BarcoHabitacion.Any(h => h.Idhabitacion == habitacion.Idhabitacion))
                        {
                            dto.BarcoHabitacion.Add(habitacion);
                        }
                    }
                }
            }

      
            await _serviceBarco.UpdateAsync(id, dto);

            TempData["Mensaje"] = Util.SweetAlertHelper.Mensaje(
                "Barco actualizado",
                "El barco se actualizó correctamente",
                Util.SweetAlertMessageType.success
            );

            return RedirectToAction("IndexAdmin");
        }
        public async Task<IActionResult> GetHabitacionesPorBarco(int id)
        {
            var habitaciones = await _serviceHabitacion.ObtenerHabitacionesPorBarcoAsync(id);
            return PartialView("_DetailHabitaciones", habitaciones);
        }
        public async Task<IActionResult> GetHabitacionesByNombre(string filtro)
        {

            var collection = await _serviceBarco.FindByNameAsync(filtro);
            Console.WriteLine(JsonSerializer.Serialize(collection)); 
            return Json(collection);

        }
        public async Task<IActionResult> addHabitacion(List<BarcoHabitacionDTO> habitaciones)
        {
            var listahabitaciones = new List<BarcoHabitacionDTO>();
            string json = "";

         
            if (TempData["BarcoCreate"] != null)
            {
                json = (string)TempData["BarcoCreate"]!;
                listahabitaciones = JsonSerializer.Deserialize<List<BarcoHabitacionDTO>>(json)!;
            }
            else
            {
                
                listahabitaciones = new List<BarcoHabitacionDTO>();
            }


            foreach (var itinerario in habitaciones)
            {

                var existingItinerario = listahabitaciones.FirstOrDefault(i => i.Id == itinerario.Id);

                if (existingItinerario != null)
                {

                    existingItinerario.Idhabitacion = itinerario.Idhabitacion;
                    existingItinerario.Cantidad = itinerario.Cantidad;
                }
                else
                {

                    listahabitaciones.Add(itinerario);
                }
            }


            json = JsonSerializer.Serialize(listahabitaciones);

            TempData["BarcoCreate"] = json;


            TempData.Keep();

            await Task.CompletedTask;

            return Ok();
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
