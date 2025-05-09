﻿using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServiceHabitacion
    {
        Task<ICollection<HabitacionDTO>> ListAsync();
        Task<HabitacionDTO> FindByIdAsync(int id);
        Task<ICollection<HabitacionDTO>> ObtenerHabitacionesPorBarcoAsync(int idBarco);
        Task<int> AddAsync(HabitacionDTO dto);
        Task<bool> ExisteNombreAsync(string nombre);
        Task<bool> ExisteNombreActAsync(string nombre, int id);
        Task UpdateAsync(int id, HabitacionDTO dto);

      

    }
}
