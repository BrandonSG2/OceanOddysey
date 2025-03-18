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
        
        public async Task<int> AddAsync(Habitacion habitacionDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Crear el objeto habitacion
                var habitacion = new Habitacion
                {
                    Nombre = habitacionDto.Nombre,
                    Detalles = habitacionDto.Detalles,
                    CapacidadMaxima = habitacionDto.CapacidadMaxima,
                    CapacidadMinima = habitacionDto.CapacidadMinima,
                    Tamanno = habitacionDto.Tamanno,
                };


                _context.Habitacion.Add(habitacion);
                await _context.SaveChangesAsync();


                


                await transaction.CommitAsync();
                return habitacion.Id;
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
                return -1;
            }
        }
        public async Task UpdateAsync(Habitacion entity)
        {
            
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                var habitacionExistente = await _context.Habitacion.FindAsync(entity.Id);

                if (habitacionExistente == null)
                    throw new KeyNotFoundException("No se encontró la habitación con el ID especificado.");

                // Actualizar las propiedades manualmente
                habitacionExistente.Nombre = entity.Nombre;
                habitacionExistente.Detalles = entity.Detalles;
                habitacionExistente.CapacidadMaxima = entity.CapacidadMaxima;
                habitacionExistente.CapacidadMinima = entity.CapacidadMinima;
                habitacionExistente.Tamanno = entity.Tamanno;

                // Guardar cambios
                await _context.SaveChangesAsync();
            
            //Las relaciones a actualizar depende de la consulta utilizada en el servicio
            //Relación de muchos a muchos solo con llave primaria compuesta
            await _context.SaveChangesAsync();
        }
        

    }
}
