using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryResumenReservacion
    {
        Task<ICollection<ResumenReservacion>> ListAsync();
        Task<ResumenReservacion> FindByIdAsync(int id);

        Task<ICollection<ResumenReservacion>> buscarXCruceroYfecha( int IDFechaCrucero);
        //Task<int> GetTotalHabitaciones(int idBarco);
    }
}
