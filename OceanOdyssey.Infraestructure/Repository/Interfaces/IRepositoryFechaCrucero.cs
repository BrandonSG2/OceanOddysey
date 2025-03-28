using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryFechaCrucero
    {


        Task<ICollection<FechaCrucero>> ListAsync();
        Task<FechaCrucero> FindByIdAsync(int id);

        Task<ICollection<FechaCrucero>> FechaXCrucero(int idCrucero);
        Task<ICollection<PrecioHabitacion>> PreciosHabitacionesPorFecha(int idFechaCrucero);
    }
}
