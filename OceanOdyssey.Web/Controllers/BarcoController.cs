using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Interfaces;
using X.PagedList.Extensions;
using System.Text.Json;
using OceanOdyssey.Infraestructure.Models;
using System.Text.Json.Serialization;
using OceanOdyssey.Web.ViewModels;
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
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceBarco.ListAsync();
            foreach (var barco in collection)
            {

                barco.TotalHabitaciones = await _serviceBarco.GetTotalHabitaciones(barco.id);
            }

            return View(collection);
        }

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

        // GET: BarcoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var @object = await _serviceBarco.FindByIdAsync(id);
            return View(@object);
        }

        // GET: BarcoController/Create
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

                // Buscar el elemento por su ID
                var habitacion = lista.FirstOrDefault(h => h.Idhabitacion == idHabitacion);
                if (habitacion != null)
                {
                    if (habitacion.Cantidad > 1)
                    {
                        // Si la cantidad es mayor a 1, restamos 1
                        habitacion.Cantidad--;
                    }
                    else
                    {
                        // Si la cantidad es 1, eliminamos el objeto de la lista
                        lista.Remove(habitacion);
                    }

                    // Serializar la lista y actualizar TempData
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

            // Verificar si estamos editando un barco
            int? idBarco = TempData["IdBarcoEditando"] as int?;
            if (idBarco != null)
            {
                detalle.Idbarco = idBarco; // Solo asignamos el ID si existe
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
        public async Task<IActionResult> Create(BarcoDTO barcoDTO)
        {
            Console.WriteLine(JsonSerializer.Serialize(barcoDTO));
            if (!ModelState.IsValid)
            {
                return View(barcoDTO);
            }

            // Verificar si el nombre del barco ya existe
            var existeNombre = await _serviceBarco.ExisteNombreAsync(barcoDTO.Nombre);
            if (existeNombre)
            {
                ModelState.AddModelError("Nombre", "El nombre del barco ya está registrado. Elige otro.");
                return View(barcoDTO);
            }

            // Obtener la lista de habitaciones desde TempData
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
            // Verificar si hay habitaciones para asociarlas al barco
            if (habitaciones.Any())
            {
                // Se asignan las habitaciones al barco
                barcoDTO.BarcoHabitacion = habitaciones;

                // Se asigna el ID del barco a cada habitación (esto es útil si es necesario en la BD)
                habitaciones.ForEach(h => h.Idbarco = barcoDTO.id);
            }

            // Intentar guardar en la base de datos
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

            return RedirectToAction("IndexAdmin"); // Redirigir tras el éxito
        }

        // GET: BarcoController/Edit/5
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

            // Guardar habitaciones en TempData si no existen
            if (TempData["BarcoCreate"] == null)
            {
                string json = JsonSerializer.Serialize(barcoDTO.BarcoHabitacion);
                TempData["BarcoCreate"] = json;
            }

            TempData.Keep("BarcoCreate"); // Mantener datos en TempData

            return View(barcoDTO);


            // ✅ Ahora la vi
            // Crear objetos BarcoHabitacionDTO a partir de las habitaciones obtenidas

        }

        // POST: BarcoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            // Guardar cambios
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
            Console.WriteLine(JsonSerializer.Serialize(collection)); // Ver en Output de la consola
            return Json(collection);

        }
        public async Task<IActionResult> addHabitacion(List<BarcoHabitacionDTO> habitaciones)
        {
            var listahabitaciones = new List<BarcoHabitacionDTO>();
            string json = "";

            // Verifica si "ItinerarioList" existe en TempData
            if (TempData["BarcoCreate"] != null)
            {
                json = (string)TempData["BarcoCreate"]!;
                listahabitaciones = JsonSerializer.Deserialize<List<BarcoHabitacionDTO>>(json)!;
            }
            else
            {
                // Si no existe, inicializa la lista vacía
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
