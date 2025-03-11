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
    public class ServicePuerto : IServicePuerto
    {

        private readonly IRepositoryPuerto _repository;
        private readonly IMapper _mapper;

        private readonly ILogger<ServiceBarco> _logger;

        public ServicePuerto(IRepositoryPuerto repository, IMapper mapper, ILogger<ServiceBarco> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<PuertoDTO> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<PuertoDTO>> ListAsync()
        {
            //obtener datos del repositorio 
            var list = await _repository.ListAsync();
           
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<PuertoDTO>>(list);
            // retorna la lista
            return collection;
        }
    }
}
