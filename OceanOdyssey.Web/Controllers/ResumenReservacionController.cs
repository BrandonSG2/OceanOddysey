using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Implementations;
using OceanOdyssey.Application.Services.Interfaces;
using OceanOdyssey.Infraestructure.Models;
using OceanOdyssey.Web.UpdateModels;
using System.Security.Claims;
using System.Text.Json;

namespace OceanOdyssey.Web.Controllers
{ 
    public class ResumenReservacionController : Controller
    {
        private readonly IServiceResumenReservacion _serviceResumenReservacion;
        private readonly IServiceCrucero _serviceCrucero;
        private readonly IServiceFechaCrucero _serviceFechaCrucero;
        private readonly IServiceComplemento _serviceComplemento;
        private readonly IServicePasajero _servicePasajero;
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServiceCambio _serviceCambio;
        public ResumenReservacionController(IServiceResumenReservacion serviceResumenReservacion,IServiceCambio serviceCambio, IServiceCrucero serviceCrucero, IServiceFechaCrucero serviceFechaCrucero, IServiceComplemento serviceComplemento, IServicePasajero servicePasajero, IServiceUsuario serviceUsuario)
        {
            _serviceResumenReservacion = serviceResumenReservacion;
            _serviceCrucero = serviceCrucero;
            _serviceFechaCrucero = serviceFechaCrucero;
            _serviceComplemento = serviceComplemento;
            _servicePasajero = servicePasajero;
            _serviceUsuario = serviceUsuario;
            _serviceCambio= serviceCambio;
        }









        [Authorize(Roles = "Cliente,Admin")]
        // GET: ResumenReservacion
        public async Task<ActionResult> Index()
        {
            var collection = await _serviceResumenReservacion.ListAsync();
            ViewBag.ListFechas = await _serviceFechaCrucero.ListAsync();
             ViewBag.Cambio= await _serviceCambio.ListAsync();
            return View(collection);
        }
        [Authorize(Roles = "Cliente,Admin")]
        [HttpGet]
        public async Task<IActionResult> buscarxCrucero(int idCrucero)
        {
            var collection = await _serviceResumenReservacion.ListAsync();
            if (idCrucero != 0)
            {
                collection = await _serviceResumenReservacion.buscarXCruceroYfecha(idCrucero) ?? new List<ResumenReservacionDTO>(); ;

            }
            return PartialView("_ListReservas", collection);
        }

        [HttpGet]
        // GET: ResumenReservacion/Details/5
        [Authorize(Roles = "Cliente,Admin")]
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



        public async Task<IActionResult> GenerarPdf(int id)
        {
           
                var rutaPdf = await _serviceResumenReservacion.GenerarPdfResumenReservacionAsync(id);

            
                if (string.IsNullOrEmpty(rutaPdf) || !System.IO.File.Exists(rutaPdf))
                {
                    return NotFound($"No se pudo encontrar el archivo PDF para la reserva con ID: {id}");
                }

              
                var fileStream = System.IO.File.OpenRead(rutaPdf);

              
                return File(fileStream, "application/pdf", $"Reserva_{id}.pdf");
            
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

                var fechaCrucero = crucero.FechaCrucero.FirstOrDefault(f => f.Id == id);

                if (fechaCrucero == null)
                {
                    return Json(new { success = false, message = "Fecha de crucero no encontrada" });
                }

                var detalle = new
                {
                    nombre = crucero.Nombre,
                    puertoSalida = crucero.Itinerario.FirstOrDefault()?.IdpuertoNavigation?.Nombre ?? "Desconocido",
                    puertoRegreso = crucero.Itinerario.LastOrDefault()?.IdpuertoNavigation?.Nombre ?? "Desconocido",
                    fechaInicio = fechaCrucero.FechaInicio.ToString("dd/MM/yyyy"),
                    fechaFin = fechaCrucero.FechaInicio.AddDays(crucero.Duracion).ToString("dd/MM/yyyy")
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
                costo = ph.Costo,
                minimo = ph.IdhabitacionNavigation.CapacidadMinima,
                maximo = ph.IdhabitacionNavigation.CapacidadMaxima
            }).ToList();

            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> AddPasajero(List<PasajeroDTO> pasajero)
        {
            var listaPasajeros = new List<PasajeroDTO>();
            string json = "";

            if (TempData["PasajeroList"] != null)
            {
                json = (string)TempData["PasajeroList"]!;
                listaPasajeros = JsonSerializer.Deserialize<List<PasajeroDTO>>(json)!;
            }

            foreach (var p in pasajero)
            {
                var existente = listaPasajeros.FirstOrDefault(x =>
                    x.Telefono == p.Telefono &&
                    x.FechaNacimiento == p.FechaNacimiento &&
                    x.Idhabitacion == p.Idhabitacion
                );

                if (existente != null)
                {
                    existente.Nombre = p.Nombre;
                    existente.Correo = p.Correo;
                    existente.Direccion = p.Direccion;
                    existente.Sexo = p.Sexo;
                }
                else
                {
                    listaPasajeros.Add(p);
                }
            }

            json = JsonSerializer.Serialize(listaPasajeros);
            TempData["PasajeroList"] = json;
            TempData.Keep();

            await Task.CompletedTask;

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> AddComplemento(List<ReservaComplementoDTO> complementos)
        {
            var listaComplementos = new List<ReservaComplementoDTO>();
            string json = "";

            if (TempData["ComplementoList"] != null)
            {
                json = (string)TempData["ComplementoList"]!;
                listaComplementos = JsonSerializer.Deserialize<List<ReservaComplementoDTO>>(json)!;
            }

            foreach (var comp in complementos)
            {
                var existente = listaComplementos.FirstOrDefault(x => x.Idcomplemento == comp.Idcomplemento);

                if (existente != null)
                {
                   
                    existente.Cantidad = comp.Cantidad;
                }
                else
                {
                
                    listaComplementos.Add(new ReservaComplementoDTO
                    {
                        Idcomplemento = comp.Idcomplemento,
                        Cantidad = comp.Cantidad
                    });
                }
            }

            json = JsonSerializer.Serialize(listaComplementos);
            TempData["ComplementoList"] = json;
            TempData.Keep();

            await Task.CompletedTask;

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> AddHabitaciones(List<int> habitaciones)
        {
            var listaHabitaciones = new List<ReservaHabitacionDTO>();
            string json = "";

            if (TempData["ReservaHabitacionData"] != null)
            {
                json = (string)TempData["ReservaHabitacionData"]!;
                listaHabitaciones = JsonSerializer.Deserialize<List<ReservaHabitacionDTO>>(json)!;
            }

            foreach (var id in habitaciones)
            {
          
                if (!listaHabitaciones.Any(x => x.Idhabitacion == id))
                {
                    listaHabitaciones.Add(new ReservaHabitacionDTO
                    {
                        Idhabitacion = id
                    });
                }
            }

            json = JsonSerializer.Serialize(listaHabitaciones);
            TempData["ReservaHabitacionData"] = json;
            TempData.Keep();

            await Task.CompletedTask;
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> TotalesReserva(int cantidadHabitaciones, decimal totalHabitaciones, decimal subtotal, decimal impuestos, decimal precioTotal)
        {
            var listaResumenReservacion = new List<ResumenReservacionDTO>();
            string json = "";

            if (TempData["ResumenReservacionData"] != null)
            {
                json = (string)TempData["ResumenReservacionData"]!;
                listaResumenReservacion = JsonSerializer.Deserialize<List<ResumenReservacionDTO>>(json)!;
            }

      
            var reservaExistente = listaResumenReservacion.FirstOrDefault();

            if (reservaExistente != null)
            {
                reservaExistente.CantidadHabitaciones = cantidadHabitaciones;
                reservaExistente.TotalHabitaciones = totalHabitaciones;
                reservaExistente.PrecioTotal = precioTotal;
                reservaExistente.Impuestos = impuestos;
                reservaExistente.TotalFinal = subtotal + impuestos;
            }
            else
            {
               
                listaResumenReservacion.Add(new ResumenReservacionDTO
                {
                    CantidadHabitaciones = cantidadHabitaciones,
                    TotalHabitaciones = totalHabitaciones,
                    PrecioTotal = precioTotal,
                    Impuestos = impuestos,
                    TotalFinal = subtotal + impuestos
                });
            }

      
            json = JsonSerializer.Serialize(listaResumenReservacion);
            TempData["ResumenReservacionData"] = json;
            TempData.Keep();

            await Task.CompletedTask;
            return Ok();
        }






        // GET: ResumenReservacion/Create
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<IActionResult> Create()
        {

            ViewBag.ListCruceros = await _serviceCrucero.ListAsync();
            ViewBag.ListCambio=await _serviceCambio.ListAsync();
            ViewBag.ListComplementos = await _serviceComplemento.ListAsync();
            return View();
        }

        // POST: ResumenReservacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<IActionResult> Create(ResumenReservacionDTO reservacionDto)
        {
            // Recuperar el crucero
            var crucero = await _serviceCrucero.FindByIdAsync(reservacionDto.Idcrucero);

            var fecha = await _serviceFechaCrucero.FindByIdAsync(reservacionDto.FechaCrucero);
            if (crucero == null)
            {
                TempData["Error"] = "Crucero no válido.";
                return RedirectToAction("Create");
            }

            reservacionDto.IdcruceroNavigation = crucero;
            reservacionDto.FechaCruceroNavigation = fecha;



         
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
            {
                TempData["Error"] = "No se pudo encontrar el identificador de usuario.";
                return RedirectToAction("Create");
            }

           
            var usuario = await _serviceUsuario.FindByIdAsyncReserva(usuarioId); 
            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction("Create");
            }

  
            reservacionDto.IdusuarioNavigation = usuario;
            reservacionDto.Idusuario = usuario.Id;


    
            List<PasajeroDTO> pasajerosList = new();
            if (TempData["PasajeroList"] != null)
            {
                string jsonPasajeros = (string)TempData["PasajeroList"];
                pasajerosList = JsonSerializer.Deserialize<List<PasajeroDTO>>(jsonPasajeros);
            }

          
            if (TempData["ComplementoList"] != null)
            {
                var jsonComplementos = (string)TempData["ComplementoList"];
                var complementos = JsonSerializer.Deserialize<List<ReservaComplementoDTO>>(jsonComplementos);
                if (complementos != null && complementos.Any())
                {
                    reservacionDto.ReservaComplemento = complementos;
                }
            }

            var pasajerosInserted = new List<PasajeroDTO>();
            foreach (var pasajero in pasajerosList)
            {
                var pasajeroId = await _servicePasajero.AddAsync(pasajero);
                if (pasajeroId != -1)
                {
                    pasajero.Id = pasajeroId;
                    pasajerosInserted.Add(pasajero);
                }
                else
                {
                    TempData["Error"] = "Error al insertar el pasajero.";
                    return RedirectToAction("Create");
                }
            }

       
            var reservaHabitaciones = new List<ReservaHabitacionDTO>();
            if (TempData["ReservaHabitacionData"] != null)
            {
                var jsonHabitaciones = (string)TempData["ReservaHabitacionData"];
                var habitaciones = JsonSerializer.Deserialize<List<ReservaHabitacionDTO>>(jsonHabitaciones);

                if (habitaciones != null)
                {
                    foreach (var habitacion in habitaciones)
                    {
                        var pasajerosPorHabitacion = pasajerosInserted
                            .Where(p => p.Idhabitacion == habitacion.Idhabitacion)
                            .ToList();

                        foreach (var pasajero in pasajerosPorHabitacion)
                        {
                            reservaHabitaciones.Add(new ReservaHabitacionDTO
                            {
                                Idhabitacion = habitacion.Idhabitacion,
                                Idpasajero = pasajero.Id
                            });
                        }
                    }
                }
            }

           
            reservacionDto.ReservaHabitacion = reservaHabitaciones;

            
            if (TempData["ResumenReservacionData"] != null)
            {
                var jsonTotales = (string)TempData["ResumenReservacionData"];
                var resumenReservacion = JsonSerializer.Deserialize<List<ResumenReservacionDTO>>(jsonTotales)?.FirstOrDefault();

                if (resumenReservacion != null)
                {
                    reservacionDto.CantidadHabitaciones = resumenReservacion.CantidadHabitaciones;
                    reservacionDto.TotalHabitaciones = resumenReservacion.TotalHabitaciones;
                    reservacionDto.PrecioTotal = resumenReservacion.PrecioTotal;
                    reservacionDto.Impuestos = resumenReservacion.Impuestos;
                    reservacionDto.TotalFinal = resumenReservacion.TotalFinal;
                }
            }

            // Guardar la reservación
            var result = await _serviceResumenReservacion.AddAsync(reservacionDto);
            if (result == -1)
            {
                TempData["Error"] = "Ocurrió un error al guardar la reservación.";
                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
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
