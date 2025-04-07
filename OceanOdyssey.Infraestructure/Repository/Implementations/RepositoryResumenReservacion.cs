﻿using Microsoft.EntityFrameworkCore;
using OceanOdyssey.Infraestructure.Data;
using OceanOdyssey.Infraestructure.Models;
using OceanOdyssey.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var habitaciones = dto.ReservaHabitacion.Select(h => new ReservaHabitacion
                    {
                        IdresumenReserva = reservacion.Id,
                        Idhabitacion = h.Idhabitacion,
                        Idpasajero = h.Idpasajero,
                    }).ToList();

                    _context.ReservaHabitacion.AddRange(habitaciones);
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
