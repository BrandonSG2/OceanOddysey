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
    public class RepositoryComplemento : IRepositoryComplemento
    {
        private readonly OceanOdysseyContext _context;

        public RepositoryComplemento(OceanOdysseyContext context)
        {
            _context = context;
        }

        public async Task<Complemento> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Complemento>()
                .Where(x => x.Id == id)
               .FirstOrDefaultAsync();


            return @object!;
        }

        public async Task<ICollection<Complemento>> ListAsync()
        {
            // select * from fecha complemento
            var collection = await _context.Set<Complemento>().ToListAsync();
            return collection;
        }
    }
}
