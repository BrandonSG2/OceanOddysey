using Microsoft.EntityFrameworkCore;
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
    public class RepositoryCrucero : IRepositoryCrucero
    {
        private readonly OceanOdysseyContext _context;

        public RepositoryCrucero(OceanOdysseyContext context)
        {
            _context = context;
        }
        public async Task<Crucero> FindByIdAsync(int id)
        {

            var @object = await _context.Set<Crucero>()
                .Include(barcocrucero => barcocrucero.IdbarcoNavigation)
                    .ThenInclude(barcohabitacion => barcohabitacion.BarcoHabitacion)
                     .ThenInclude(habitacion => habitacion.IdhabitacionNavigation)
                      .ThenInclude(habitacionPrecio => habitacionPrecio!.PrecioHabitacion)
                .Include(FechaCrucero => FechaCrucero.FechaCrucero)
                .Include(ItinerarioCrucero => ItinerarioCrucero.Itinerario)
                .ThenInclude(Itinerario => Itinerario.IdpuertoNavigation)
                .Where(x => x.Id == id)
                .FirstAsync();
            if (@object != null)
            {
                foreach (var barcoHabitacion in @object.IdbarcoNavigation.BarcoHabitacion)
                {
                    barcoHabitacion.IdhabitacionNavigation!.PrecioHabitacion =
                        barcoHabitacion.IdhabitacionNavigation.PrecioHabitacion
                            .Where(p => @object.FechaCrucero.Any(f => f.Id == p.IdFechaCrucero))
                            .ToList();
                }
            }
            Console.Write(@object);

            return @object!;
        }
        public async Task<ICollection<Crucero>> ListAsync()
        {
            // select * from barco
            var collection = await _context.Set<Crucero>().ToListAsync();
            return collection;
        }



        public async Task<int> AddAsync(Crucero cruceroDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
           
                var crucero = new Crucero
                {
                    Nombre = cruceroDto.Nombre,
                    Imagen = cruceroDto.Imagen,
                    Duracion = cruceroDto.Duracion,
                    Idbarco = cruceroDto.Idbarco,
                    Disponible = cruceroDto.Disponible,
                };


                _context.Crucero.Add(crucero);
                await _context.SaveChangesAsync();


                if (cruceroDto.Itinerario != null && cruceroDto.Itinerario.Any())
                {

                    var itinerarios = cruceroDto.Itinerario.Select(it => new Itinerario
                    {
                        Idcrucero = crucero.Id,
                        Idpuerto = it.Idpuerto,
                        Descripcion = it.Descripcion,
                        Dia = it.Dia
                    }).ToList();


                    _context.Itinerario.AddRange(itinerarios);
                    await _context.SaveChangesAsync();
                }


                if (cruceroDto.FechaCrucero != null && cruceroDto.FechaCrucero.Any())
                {
                    foreach (var fecha in cruceroDto.FechaCrucero)
                    {
                        var fechaCrucero = new FechaCrucero
                        {
                            Idcrucero = crucero.Id,
                            FechaInicio = fecha.FechaInicio,
                            // FechaLimite = fecha.FechaLimite
                        };

                        _context.FechaCrucero.Add(fechaCrucero);
                        await _context.SaveChangesAsync();


                        if (fecha.PrecioHabitacion != null && fecha.PrecioHabitacion.Any())
                        {
                            var precios = fecha.PrecioHabitacion.Select(hab => new PrecioHabitacion
                            {
                                IdFechaCrucero = fechaCrucero.Id,
                                Idhabitacion = hab.Idhabitacion,
                                Costo = hab.Costo
                            }).ToList();

                            _context.PrecioHabitacion.AddRange(precios);
                            await _context.SaveChangesAsync();
                        }
                    }
                }


                await transaction.CommitAsync();
                return crucero.Id;
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
                return -1;
            }
        }




    }
}
