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
    public class RepositoryFechaCrucero : IRepositoryFechaCrucero
    {

        private readonly OceanOdysseyContext _context;

        public RepositoryFechaCrucero(OceanOdysseyContext context)
        {
            _context = context;
        }
        public async Task<ICollection<FechaCrucero>> FechaXCrucero(int idCrucero)
        {
           
                return await _context.FechaCrucero
                    .Where(fc => fc.Idcrucero == idCrucero)
                    .Include(fc => fc.IdcruceroNavigation)  
                    .Include(fc => fc.PrecioHabitacion)     
                    .Include(fc => fc.ResumenReservacion)   
                    .ToListAsync();
            

        }
        public async Task<ICollection<PrecioHabitacion>> PreciosHabitacionesPorFecha(int idFechaCrucero)
        {
            return await _context.PrecioHabitacion
                .Where(ph => ph.IdFechaCruceroNavigation!.Id == idFechaCrucero)
                .Include(ph => ph.IdFechaCruceroNavigation)
                .Include(ph => ph.IdhabitacionNavigation) 
                .ToListAsync();
        }

   


        public async Task<FechaCrucero> FindByIdAsync(int id)
        {

            var @object = await _context.Set<FechaCrucero>()

                .Where(x => x.Id == id)
                .Include(x=> x.IdcruceroNavigation)
                .FirstOrDefaultAsync();


            return @object!;
        }

        public async Task<ICollection<FechaCrucero>> ListAsync()
        {
            // select * from fecha crucero
            var collection = await _context.Set<FechaCrucero>().
                Include(x => x.IdcruceroNavigation)
                .ToListAsync();
            return collection;
        }

    }
}
