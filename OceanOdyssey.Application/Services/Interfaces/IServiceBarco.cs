using OceanOdyssey.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServiceBarco
    {
        Task<ICollection<BarcoDTO>> ListAsync();
        Task<BarcoDTO> FindByIdAsync(int id);
        Task<int> GetTotalHabitaciones(int idBarco);
        Task<int> AddAsync(BarcoDTO dto);
        Task<bool> ExisteNombreAsync(string nombre);
        Task<bool> ExisteNombreActAsync(string nombre, int id);
        Task UpdateAsync(int id, BarcoDTO dto);

        Task<ICollection<HabitacionDTO>> FindByNameAsync(string nombre);
    }
}
