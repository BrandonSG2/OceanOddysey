using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryBarco
    {
        Task<ICollection<Barco>> ListAsync();
        Task<Barco> FindByIdAsync(int id);
        Task UpdateAsync(Barco entity);
        Task<int> AddAsync(Barco dto);
        Task EliminarRelacionesHabitaciones(int idBarco);

        Task<int> GetTotalHabitaciones(int idBarco);
        Task<ICollection<Habitacion>> FindByNameAsync(string nombre);
    }
}
