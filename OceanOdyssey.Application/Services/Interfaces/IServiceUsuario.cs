﻿using OceanOdyssey.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServiceUsuario
    {
        Task<ICollection<UsuarioDTO>> FindByDescriptionAsync(string description);
        Task<ICollection<UsuarioDTO>> ListAsync();
        Task<ICollection<ResumenReservacionDTO>> HistorialUsuario(string id);
        Task<UsuarioDTO> FindByIdAsync(string id);
        Task<UsuarioDTO> FindByIdAsyncReserva(string id);
        Task<UsuarioDTO> LoginAsync(string id, string password);
        Task<string> AddAsync(UsuarioDTO dto);
        Task DeleteAsync(string id);
        Task UpdateAsync(string id, UsuarioDTO dto);
    }
}
