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
    public class RepositoryPuerto : IRepositoryPuerto
    {
        private readonly OceanOdysseyContext _context;

        public RepositoryPuerto(OceanOdysseyContext context)
        {
            _context = context;
        }

        public Task<Puerto> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Puerto>> ListAsync()
        {
            
            var collection = await _context.Set<Puerto>().ToListAsync();
          
            return collection;
        }
    }
}
