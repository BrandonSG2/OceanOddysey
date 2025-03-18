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
    public class RepositoryBarcoHabitacion : IRepositoryBarcoHabitacion
    {
        private readonly OceanOdysseyContext _context;

        public RepositoryBarcoHabitacion(OceanOdysseyContext context)
        {
            _context = context;
        }
        public async Task<ICollection<BarcoHabitacion>> ObtenerHabitacionesPorNaveAsync(int idBarco)
        {
            return await _context.BarcoHabitacion
    .Where(bh => bh.IdbarcoNavigation!.Id == idBarco)
    .ToListAsync();
        }
    }
}
