using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPasajero
    {
        Task<ICollection<Pasajero>> ListAsync();
        Task<Pasajero> FindByIdAsync(int id);

        Task<int> AddAsync(Pasajero dto);
    }
}
