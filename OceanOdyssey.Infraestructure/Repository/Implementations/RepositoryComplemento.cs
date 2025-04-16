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
        public async Task<int> AddAsync(Complemento habitacionDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var complemento = new Complemento
                {
                    Nombre = habitacionDto.Nombre,
                    Detalle = habitacionDto.Detalle,
                   Aplicado=habitacionDto.Aplicado,
                    Precio = habitacionDto.Precio,
                };


                _context.Complemento.Add(complemento);
                await _context.SaveChangesAsync();





                await transaction.CommitAsync();
                return complemento.Id;
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
                return -1;
            }
        }
        public async Task UpdateAsync(Complemento entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var complementoexistente = await _context.Complemento.FindAsync(entity.Id);

            if (complementoexistente == null)
                throw new KeyNotFoundException("No se encontró la habitación con el ID especificado.");


            complementoexistente.Nombre = entity.Nombre;
            complementoexistente.Detalle = entity.Detalle;
            complementoexistente.Precio = entity.Precio;
            complementoexistente.Aplicado=entity.Aplicado;


            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();
        }
    }
}
