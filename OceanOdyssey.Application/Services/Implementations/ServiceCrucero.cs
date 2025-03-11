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
    public class ServiceCrucero:IServiceCrucero
    {
        private readonly IRepositoryCrucero _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceCrucero> _logger;
        public ServiceCrucero(IRepositoryCrucero repository, IMapper mapper, ILogger<ServiceCrucero> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<CruceroDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<CruceroDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<CruceroDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<CruceroDTO>>(list);
            // retorna la lista
            return collection;
        }


        public async Task<int> AddAsync(CruceroDTO dto)
        {
            // Mapear el DTO de Crucero a la entidad Crucero
            var cruceroMapped = _mapper.Map<Crucero>(dto);

            // Mapear los itinerarios de DTO a entidades
            

            // Llamar al repositorio para agregar el crucero, pasando el itinerario mapeado como una lista de objetos Itinerario
            return await _repository.AddAsync(cruceroMapped);
        }


    }

}
