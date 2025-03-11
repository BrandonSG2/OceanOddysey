using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPuerto
    {

        Task<ICollection<Puerto>> ListAsync();
        Task<Puerto> FindByIdAsync(int id);

    }
}
