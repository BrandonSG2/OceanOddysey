using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryComplemento
    {
        Task<ICollection<Complemento>> ListAsync();
        Task<Complemento> FindByIdAsync(int id);
    }
}
