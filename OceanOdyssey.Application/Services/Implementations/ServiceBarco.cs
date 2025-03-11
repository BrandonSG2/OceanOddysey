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
    public class ServiceBarco : IServiceBarco
    {
        private readonly IRepositoryBarco _repository;
        private readonly IMapper _mapper;

        private readonly ILogger<ServiceBarco> _logger;

        public ServiceBarco(IRepositoryBarco repository, IMapper mapper, ILogger<ServiceBarco> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BarcoDTO> FindByIdAsync(int id)
        {

            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<BarcoDTO>(@object);
            return objectMapped;


        }

        public async Task<ICollection<BarcoDTO>> ListAsync()
        {
           //obtener datos del repositorio 
           var list = await _repository.ListAsync();
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<BarcoDTO>>(list);
            // retorna la lista
            return collection;
        }
        public async Task<int> GetTotalHabitaciones(int idBarco)
        {
            int nextReceipt = await _repository.GetTotalHabitaciones(idBarco);
            return nextReceipt ;
        }
    }
}
