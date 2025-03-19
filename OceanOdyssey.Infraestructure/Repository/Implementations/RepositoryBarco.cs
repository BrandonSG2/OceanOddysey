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
    public class RepositoryBarco : IRepositoryBarco
    {
        private readonly OceanOdysseyContext _context;

        public RepositoryBarco(OceanOdysseyContext context)
        {
            _context = context;
        }

        public async Task<Barco> FindByIdAsync(int id)
        {

            var @object = await _context.Set<Barco>()
                .Include(barcoHabitacion => barcoHabitacion.BarcoHabitacion)
                    .ThenInclude(habitacion => habitacion.IdhabitacionNavigation)
                .Where(x => x.Id == id)
                .FirstAsync(); 
          

            return @object!;
        }
        public async Task EliminarRelacionesHabitaciones(int idBarco)
        {
            var barco = await _context.Barco
                .Include(b => b.BarcoHabitacion)
                .FirstOrDefaultAsync(b => b.Id == idBarco);

            if (barco != null && barco.BarcoHabitacion.Any())
            {
                _context.BarcoHabitacion.RemoveRange(barco.BarcoHabitacion);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<ICollection<Barco>> ListAsync()
        {
            // select * from barco
            var collection = await _context.Set<Barco>().ToListAsync();
            return collection;
        }

        public async Task<int> GetTotalHabitaciones(int idBarco)
        {
            int totalHabitaciones = 0;

            string sql = string.Format("SELECT SUM(Cantidad) AS TotalHabitaciones FROM BarcoHabitacion WHERE IDBarco = {0};", idBarco);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            System.Data.Common.DbConnection connection = _context.Database.GetDbConnection();
            System.Data.Common.DbProviderFactory dbFactory = System.Data.Common.DbProviderFactories.GetFactory(connection!)!;
            using (var cmd = dbFactory!.CreateCommand())
            {
                cmd!.Connection = connection;
                cmd.CommandText = sql;
                using (System.Data.Common.DbDataAdapter adapter = dbFactory.CreateDataAdapter()!)
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            totalHabitaciones = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            return await Task.FromResult(totalHabitaciones);
            

           
        }
        public async Task<ICollection<Habitacion>> FindByNameAsync(string nombre)
        {
            var collection = await _context
                                         .Set<Habitacion>()
                                         .Where(p => p.Nombre.Contains(nombre))
                                         .ToListAsync();
            return collection;
        }

        public async Task<int> AddAsync(Barco barcodto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                _context.Barco.Add(barcodto);
                await _context.SaveChangesAsync(); 

                await transaction.CommitAsync();
                return barcodto.Id; 
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return -1; 
            }
        }
        public async Task UpdateAsync(Barco entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var habitacionExistente = await _context.Habitacion.FindAsync(entity.Id);

            if (habitacionExistente == null)
                throw new KeyNotFoundException("No se encontró la habitación con el ID especificado.");

           

            _context.Barco.Update(entity); 

          
            if (entity.BarcoHabitacion != null && entity.BarcoHabitacion.Any())
            {
                _context.BarcoHabitacion.AddRange(entity.BarcoHabitacion);
            }
            
            await _context.SaveChangesAsync();

            
            await _context.SaveChangesAsync();
        }
    }
}
