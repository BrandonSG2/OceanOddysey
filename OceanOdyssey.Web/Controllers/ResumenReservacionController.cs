using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Implementations;
using OceanOdyssey.Application.Services.Interfaces;
using OceanOdyssey.Web.UpdateModels;

namespace OceanOdyssey.Web.Controllers
{
    public class ResumenReservacionController : Controller
    {
        private readonly IServiceResumenReservacion _serviceResumenReservacion;
        private readonly IServiceCrucero _serviceCrucero;
        private readonly IServiceFechaCrucero _serviceFechaCrucero;
        private readonly IServiceComplemento _serviceComplemento;

        public ResumenReservacionController(IServiceResumenReservacion serviceResumenReservacion, IServiceCrucero serviceCrucero, IServiceFechaCrucero serviceFechaCrucero, IServiceComplemento serviceComplemento)
        {
            _serviceResumenReservacion = serviceResumenReservacion;
            _serviceCrucero = serviceCrucero;
            _serviceFechaCrucero = serviceFechaCrucero;
            _serviceComplemento = serviceComplemento;
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

        [HttpGet]
        public async Task<JsonResult> GetDetalleCrucero(int id)
        {
            try
            {
                var crucero = await _serviceCrucero.FindByIdAsyncCrucero(id);

                if (crucero == null)
                {
                    return Json(new { success = false, message = "Crucero no encontrado" });
                }

                var detalle = new
                {
                    nombre = crucero.Nombre,
                    puertoSalida = crucero.Itinerario.FirstOrDefault()?.IdpuertoNavigation?.Nombre ?? "Desconocido",
                    puertoRegreso = crucero.Itinerario.LastOrDefault()?.IdpuertoNavigation?.Nombre ?? "Desconocido",
                    fechaInicio = crucero.FechaCrucero.FirstOrDefault()?.FechaInicio.ToString("dd/MM/yyyy") ?? "Desconocida",
                    fechaFin = crucero.FechaCrucero.FirstOrDefault()?.FechaInicio
                        .AddDays(crucero.Duracion).ToString("dd/MM/yyyy") ?? "Desconocida"
                };

                return Json(new { success = true, data = detalle });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        [HttpGet]
        public async Task<IActionResult> GetFechasXCrucero(int idCrucero)
        {
            var fechas = await _serviceFechaCrucero.FechaXCrucero(idCrucero);

            var resultado = fechas.Select(f => new
            {
                value = f.Id,
                text = f.FechaInicio.ToString("dd-MM-yyyy")
            }).ToList();

            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> GetPreciosHabitacionesPorFecha(int idFechaCrucero)
        {
            var preciosDTO = await _serviceFechaCrucero.PreciosHabitacionesPorFecha(idFechaCrucero);

            var resultado = preciosDTO.Select(ph => new
            {
                id = ph.Id,
                nombreHabitacion = ph.IdhabitacionNavigation.Nombre,
                idHabitacion = ph.IdhabitacionNavigation.Id,
                costo = ph.Costo
            }).ToList();

            return Json(resultado);
        }



        // GET: ResumenReservacion/Create
        public async Task<IActionResult> Create()
        {

            ViewBag.ListCruceros = await _serviceCrucero.ListAsync();

            ViewBag.ListComplementos = await _serviceComplemento.ListAsync();
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


        [HttpPost]
        public IActionResult ActualizarPasajeros([FromBody] List<PasajeroDTO> pasajeros)
        {
            try
            {
                return Json(new { success = true, message = "Pasajeros actualizados correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }




        [HttpPost]
        public IActionResult ActualizarTotalHabitaciones([FromBody] ResumenReservacionDTO data)
        {
            try
            {


                return Json(new { success = true, message = "Total de habitaciones actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        [HttpPost]
        public IActionResult ActualizarTotalComplementos([FromBody] ResumenReservacionDTO data)
        {
            try
            {


                return Json(new { success = true, message = "Total de complementos actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult ActualizarTotales([FromBody] ResumenReservacionDTO data)
        {
            try
            {

                return Json(new { success = true, message = "Totales actualizados correctamente", data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }






    }
}
