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
    public class ServiceFechaCrucero : IServiceFechaCrucero
    {
        private readonly IRepositoryFechaCrucero _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceFechaCrucero> _logger;
        public ServiceFechaCrucero(IRepositoryFechaCrucero repository, IMapper mapper, ILogger<ServiceFechaCrucero> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<FechaCruceroDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<FechaCruceroDTO>>(list);
            // retorna la lista
            return collection;
        }
    }
}
