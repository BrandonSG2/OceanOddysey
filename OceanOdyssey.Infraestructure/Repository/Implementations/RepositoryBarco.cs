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


    }
}
