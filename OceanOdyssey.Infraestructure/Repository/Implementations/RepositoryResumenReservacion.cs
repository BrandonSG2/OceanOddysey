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
    public class RepositoryResumenReservacion : IRepositoryResumenReservacion
    {

        private readonly OceanOdysseyContext _context;

        public RepositoryResumenReservacion(OceanOdysseyContext context)
        {
            _context = context;
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
