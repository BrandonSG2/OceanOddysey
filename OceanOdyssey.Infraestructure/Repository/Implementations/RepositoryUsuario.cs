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
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly OceanOdysseyContext _context;

        public RepositoryUsuario(OceanOdysseyContext context)
        {
            _context = context;
        }
        public async Task<string> AddAsync(Usuario entity)
        {
            await _context.Set<Usuario>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Email;
        }

        public async Task DeleteAsync(string id)
        {

            var @object = await FindByIdAsync(id);
            _context.Remove(@object);
            _context.SaveChanges();
        }

        public async Task<ICollection<Usuario>> FindByDescriptionAsync(string description)
        {
            var collection = await _context
                                         .Set<Usuario>()
                                         .Where(p => p.Nombre.Contains(description))
                                         .ToListAsync();
            return collection;
        }

        public async Task<Usuario> FindByIdAsync(string id)
        {
            var @object = await _context.Set<Usuario>().FindAsync(id);

            return @object!;
        }

        public async Task<ICollection<Usuario>> ListAsync()
        {
            var collection = await _context.Set<Usuario>()
                                          
                                          .ToListAsync();
            return collection;
        }
        public async Task<ICollection<ResumenReservacion>> HistorialUsuario(string id)
        {
            int ID=int.Parse(id);
            var collection = await _context.Set<ResumenReservacion>()
                                    .Where(r => r.Idusuario == ID)
                                    .ToListAsync();
            return collection;
        }
        public async Task<Usuario> LoginAsync(string id, string password)
        {
            var @object = await _context.Set<Usuario>()
                                      
                                        .Where(p => p.Email == id && p.Clave == password)
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
