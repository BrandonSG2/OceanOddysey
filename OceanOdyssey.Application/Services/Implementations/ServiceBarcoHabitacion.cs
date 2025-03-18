using AutoMapper;
using Microsoft.Extensions.Logging;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Interfaces;
using OceanOdyssey.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Implementations
{
    public class ServiceBarcoHabitacion : IServiceBarcoHabitacion
    {
        private readonly IRepositoryBarcoHabitacion _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceHabitacion> _logger;
        public ServiceBarcoHabitacion(IRepositoryBarcoHabitacion repository, IMapper mapper, ILogger<ServiceHabitacion> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ICollection<BarcoHabitacionDTO>> ObtenerHabitacionesPorNaveAsync(int idBarco)
        {
            var habitaciones = await _repository.ObtenerHabitacionesPorNaveAsync(idBarco);

            var habitacionesDto = _mapper.Map<ICollection<BarcoHabitacionDTO>>(habitaciones);

            return (ICollection<BarcoHabitacionDTO>)habitacionesDto;
        }
    }
}
