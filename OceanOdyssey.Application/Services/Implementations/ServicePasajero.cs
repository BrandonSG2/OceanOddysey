using AutoMapper;
using Microsoft.Extensions.Logging;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Application.Services.Interfaces;
using OceanOdyssey.Infraestructure.Models;
using OceanOdyssey.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Implementations
{
    public class ServicePasajero : IServicePasajero
    {

        private readonly IRepositoryPasajero _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServicePasajero> _logger;

        public ServicePasajero(IRepositoryPasajero repository, IMapper mapper, ILogger<ServicePasajero> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> AddAsync(PasajeroDTO dto)
        {
            var habitacionmapped = _mapper.Map<Pasajero>(dto);
            return await _repository.AddAsync(habitacionmapped);
        }

        public Task<PasajeroDTO> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<PasajeroDTO>> ListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
