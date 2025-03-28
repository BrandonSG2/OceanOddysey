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
    public class RepositoryPais : IRepositoryPais
    {
        private readonly OceanOdysseyContext _context;

        public RepositoryPais(OceanOdysseyContext context)
        {
            _context = context;
        }
        public async Task<Pais> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Pais>()

               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();


            return @object!;
        }

        public async Task<ICollection<Pais>> ListAsync()
        {
            var collection = await _context.Set<Pais>().ToListAsync();
            return collection;
        }
    }
}
