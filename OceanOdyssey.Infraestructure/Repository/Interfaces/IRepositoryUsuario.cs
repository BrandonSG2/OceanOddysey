using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuario
    {
        Task<ICollection<Usuario>> FindByDescriptionAsync(string description);
        Task<ICollection<Usuario>> ListAsync();
        Task<Usuario> FindByIdAsync(string id);
        Task<Usuario> FindByIdAsyncReserva(string id);
        Task<ICollection<ResumenReservacion>> HistorialUsuario(string id);
        Task<Usuario> LoginAsync(string id, string password);
        Task<string> AddAsync(Usuario entity);
        Task DeleteAsync(string id);
        Task UpdateAsync();
    }
}
