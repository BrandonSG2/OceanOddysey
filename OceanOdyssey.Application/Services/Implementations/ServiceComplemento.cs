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
    public class ServiceComplemento : IServiceComplemento
    {
        private readonly IRepositoryComplemento _repository;
        private readonly IMapper _mapper;

        private readonly ILogger<ServiceComplemento> _logger;

        public ServiceComplemento(IRepositoryComplemento repository, IMapper mapper, ILogger<ServiceComplemento> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ComplementoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ComplementoDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<ComplementoDTO>> ListAsync()
        {
            //obtener datos del repositorio 

            var list = await _repository.ListAsync();
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<ComplementoDTO>>(list);
            // retorna la lista
            return collection;
        }
        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            var habitaciones = await _repository.ListAsync();
            return habitaciones.Any(h => h.Nombre.ToLower() == nombre.ToLower());
        }
        public async Task<bool> ExisteNombreActAsync(string nombre, int id)
        {
            var habitaciones = await _repository.ListAsync();
            return habitaciones.Any(h => h.Nombre.ToLower() == nombre.ToLower() && h.Id != id);
        }
        public async Task<int> AddAsync(ComplementoDTO dto)
        {

            var habitacionmapped = _mapper.Map<Complemento>(dto);



            return await _repository.AddAsync(habitacionmapped);
        }
        public async Task UpdateAsync(int id, ComplementoDTO dto)
        {

            var @object = await _repository.FindByIdAsync(id);

            var entity = _mapper.Map(dto, @object!);
            await _repository.UpdateAsync(entity);
        }
    }
}
