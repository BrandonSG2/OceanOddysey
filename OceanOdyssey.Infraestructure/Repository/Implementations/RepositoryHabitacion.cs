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
    public class RepositoryHabitacion:IRepositoryHabitacion
    {
        private readonly OceanOdysseyContext _context;

        public RepositoryHabitacion(OceanOdysseyContext context)
        {
            _context = context;
        }

        public async Task<Habitacion> FindByIdAsync(int id)
        {

            var @object = await _context.Set<Habitacion>()
               
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();


            return @object!;
        }

        public async Task<ICollection<Habitacion>> ListAsync()
        {
            // select * from barco
            var collection = await _context.Set<Habitacion>().ToListAsync();
            return collection;
        }



        public async Task<ICollection<Habitacion>> ObtenerHabitacionesPorBarcoAsync(int idBarco)
        {
            return await _context.Habitacion
                .Where(h => h.BarcoHabitacion.Any(bh => bh.IdbarcoNavigation!.Id == idBarco)) 
                .ToListAsync();
        }


    }
}
