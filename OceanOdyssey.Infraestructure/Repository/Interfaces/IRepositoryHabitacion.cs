using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryHabitacion
    {
        Task<ICollection<Habitacion>> ListAsync();
        Task<Habitacion> FindByIdAsync(int id);
        Task<ICollection<Habitacion>> ObtenerHabitacionesPorBarcoAsync(int idBarco);
        Task<int> AddAsync(Habitacion dto);
        Task UpdateAsync(Habitacion entity);
        
    }
}
