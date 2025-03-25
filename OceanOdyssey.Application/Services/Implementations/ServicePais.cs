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
    public class ServicePais : IServicePais
    {
        private readonly IRepositoryPais _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceHabitacion> _logger;
        public ServicePais(IRepositoryPais repository, IMapper mapper, ILogger<ServiceHabitacion> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<PaisDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<PaisDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<PaisDTO>> ListAsync()
        { //obtener datos del repositorio 
            var list = await _repository.ListAsync();
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<PaisDTO>>(list);
            // retorna la lista
            return collection;
        }
    }
}
