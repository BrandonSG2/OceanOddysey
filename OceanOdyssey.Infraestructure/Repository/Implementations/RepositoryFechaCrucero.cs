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
        public async Task<ICollection<FechaCrucero>> ListAsync()
        {
            var collection = await _context.Set<FechaCrucero>()
        .Include(fc => fc.IdcruceroNavigation) // Incluir datos del Crucero
        .ToListAsync();

            return collection;
        }
    }
}
