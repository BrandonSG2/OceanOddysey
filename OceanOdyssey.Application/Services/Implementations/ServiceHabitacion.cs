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
    public class ServiceHabitacion:IServiceHabitacion
    {
        private readonly IRepositoryHabitacion  _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceHabitacion> _logger;
        public ServiceHabitacion(IRepositoryHabitacion repository, IMapper mapper, ILogger<ServiceHabitacion> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<HabitacionDTO> FindByIdAsync(int id)
        {

            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<HabitacionDTO>(@object);
            return objectMapped;


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
        public async Task<ICollection<HabitacionDTO>> ObtenerHabitacionesPorBarcoAsync(int idBarco)
        {
        
            var habitaciones = await _repository.ObtenerHabitacionesPorBarcoAsync(idBarco);

            var habitacionesDto = _mapper.Map<ICollection<HabitacionDTO>>(habitaciones);

            return habitacionesDto;
        }
        


        public async Task<ICollection<HabitacionDTO>> ListAsync()
        {
            //obtener datos del repositorio 
            var list = await _repository.ListAsync();
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<HabitacionDTO>>(list);
            // retorna la lista
            return collection;
        }
        public async Task<int> AddAsync(HabitacionDTO dto)
        {
          
            var habitacionmapped = _mapper.Map<Habitacion>(dto);
  

         
            return await _repository.AddAsync(habitacionmapped);
        }
        public async Task UpdateAsync(int id, HabitacionDTO dto)
        {
           
            var @object = await _repository.FindByIdAsync(id);
         
            var entity = _mapper.Map(dto, @object!);
            await _repository.UpdateAsync(entity);
        }

        
    }
}
