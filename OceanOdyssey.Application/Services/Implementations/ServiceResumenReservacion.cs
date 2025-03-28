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
    public class ServiceResumenReservacion : IServiceResumenReservacion
    {
        private readonly IRepositoryResumenReservacion _repository;
        private readonly IMapper _mapper;

        private readonly ILogger<ServiceBarco> _logger;

        public ServiceResumenReservacion(IRepositoryResumenReservacion repository, IMapper mapper, ILogger<ServiceBarco> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<ResumenReservacionDTO>> buscarXCruceroYfecha(int IDFechaCrucero)
        {
            var list = await _repository.buscarXCruceroYfecha(IDFechaCrucero);
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<ResumenReservacionDTO>>(list);
            // retorna la lista
            return collection;
        }

        public async Task<ResumenReservacionDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ResumenReservacionDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<ResumenReservacionDTO>> ListAsync()
        {
            //obtener datos del repositorio 
            var list = await _repository.ListAsync();
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<ResumenReservacionDTO>>(list);
            // retorna la lista
            return collection;
        }
    }
}
