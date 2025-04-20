using Microsoft.EntityFrameworkCore;
using OceanOdyssey.Infraestructure.Data;
using OceanOdyssey.Infraestructure.Models;
using OceanOdyssey.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Fluent;


using System.Globalization;
using System.ComponentModel;


namespace OceanOdyssey.Infraestructure.Repository.Implementations
{
    public class RepositoryResumenReservacion : IRepositoryResumenReservacion
    {

        private readonly OceanOdysseyContext _context;

        public RepositoryResumenReservacion(OceanOdysseyContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(ResumenReservacion dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
              
                var crucero = await _context.Set<Crucero>().FindAsync(dto.Idcrucero);
                if (crucero == null)
                {
                    throw new Exception("Crucero no encontrado");
                }

                var idBarco = crucero.Idbarco;

            
                var reservacion = new ResumenReservacion
                {
                    Idusuario = dto.Idusuario,
                    Idcrucero = dto.Idcrucero,
                    FechaReservacion = DateOnly.FromDateTime(DateTime.Now),
                    CantidadHabitaciones = dto.CantidadHabitaciones,
                    PrecioTotal = dto.PrecioTotal,
                    Impuestos = dto.Impuestos,
                    TotalFinal = dto.TotalFinal,
                    Estado = dto.Estado,
                    FechaCrucero = dto.FechaCrucero,
                    FechaPago = dto.FechaPago,
                    TotalHabitaciones = dto.TotalHabitaciones
                };

                _context.ResumenReservacion.Add(reservacion);
                await _context.SaveChangesAsync();
                if (dto.ReservaHabitacion != null && dto.ReservaHabitacion.Any())
                {
                    var habitacionesParaGuardar = new List<ReservaHabitacion>();
                    var barcoHabitacionesParaActualizar = new List<BarcoHabitacion>();

                    foreach (var reserva in dto.ReservaHabitacion)
                    {
                        
                        habitacionesParaGuardar.Add(new ReservaHabitacion
                        {
                            IdresumenReserva = reservacion.Id,
                            Idhabitacion = reserva.Idhabitacion,
                            Idpasajero = reserva.Idpasajero
                        });

                       
                        var barcoHabitacion = await _context.BarcoHabitacion
                            .FirstOrDefaultAsync(bh => bh.Idbarco == idBarco && bh.Idhabitacion == reserva.Idhabitacion);

                        if (barcoHabitacion != null)
                        {
                            if (barcoHabitacion.Cantidad <= 0)
                            {
                                throw new Exception($"No hay disponibilidad para la habitación {reserva.Idhabitacion}");
                            }

                            barcoHabitacion.Cantidad--;


                            if (!barcoHabitacionesParaActualizar.Contains(barcoHabitacion))
                            {
                                barcoHabitacionesParaActualizar.Add(barcoHabitacion);
                            }
                        }
                        else
                        {
                            throw new Exception($"No se encontró la relación entre el barco y la habitación {reserva.Idhabitacion}");
                        }
                    }

                    _context.ReservaHabitacion.AddRange(habitacionesParaGuardar);
                    _context.BarcoHabitacion.UpdateRange(barcoHabitacionesParaActualizar);
                    await _context.SaveChangesAsync();
                }

               
                if (dto.ReservaComplemento != null && dto.ReservaComplemento.Any())
                {
                    var complementos = dto.ReservaComplemento.Select(c => new ReservaComplemento
                    {
                        IdresumenReserva = reservacion.Id,
                        Idcomplemento = c.Idcomplemento,
                        Cantidad = c.Cantidad,
                    }).ToList();

                    _context.ReservaComplemento.AddRange(complementos);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return reservacion.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return -1;
            }
        }



        public async Task<ICollection<ResumenReservacion>> buscarXCruceroYfecha(int IDFechaCrucero)
        {
            var collection = await _context.Set<ResumenReservacion>()
        .Where(r => r.FechaCrucero == IDFechaCrucero)
        .ToListAsync();
            return collection;
        }
        public async Task<ResumenReservacion> FindByIdAsync(int id)
        {
            var @object = await _context.Set<ResumenReservacion>()
                .Include(x => x.ReservaComplemento)
                    .ThenInclude(x => x.IdcomplementoNavigation)
                .Include(x => x.IdcruceroNavigation)
                    .ThenInclude(x => x.Itinerario)
                    .ThenInclude(x => x.IdpuertoNavigation)
                .Include(x => x.IdcruceroNavigation)
                    .ThenInclude(x => x.IdbarcoNavigation)
                .Include(x => x.IdcruceroNavigation)
                    .ThenInclude(x => x.FechaCrucero)
                .Include(x => x.ReservaHabitacion)
                    .ThenInclude(x => x.IdhabitacionNavigation)
                    .ThenInclude(x => x.PrecioHabitacion)
                    .ThenInclude(x => x.IdFechaCruceroNavigation)
                .Include(x => x.ReservaHabitacion)
                    .ThenInclude(x => x.IdpasajeroNavigation)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (@object != null)
            {
                var fechaCrucero = @object.FechaCruceroNavigation?.FechaInicio;


                foreach (var reserva in @object.ReservaHabitacion)
                {
                    reserva.IdhabitacionNavigation!.PrecioHabitacion = reserva.IdhabitacionNavigation.PrecioHabitacion
                        .Where(p => p.IdFechaCruceroNavigation != null && p.IdFechaCruceroNavigation.FechaInicio == fechaCrucero)
                        .ToList();
                }
            }

            return @object!;
        }






        public async Task<ICollection<ResumenReservacion>> ListAsync()
        {


            var collection = await _context.Set<ResumenReservacion>()
               .Include(x => x.FechaCruceroNavigation)
                .ToListAsync();
            return collection;
        }



    }




}

