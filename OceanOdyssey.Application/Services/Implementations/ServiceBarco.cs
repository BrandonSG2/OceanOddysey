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
        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            var barcos = await _repository.ListAsync();
            return barcos.Any(h => h.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task<bool> ExisteNombreActAsync(string nombre, int id)
        {
            var barcos = await _repository.ListAsync();
            return barcos.Any(h => h.Nombre.ToLower() == nombre.ToLower() && h.Id != id);
        }
        public async Task<int> AddAsync(BarcoDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "El DTO no puede ser nulo");

            // Mapear el DTO a la entidad Barco
            var barco = _mapper.Map<Barco>(dto);

            // Asegurar que las relaciones se establecen correctamente
            if (dto.BarcoHabitacion != null && dto.BarcoHabitacion.Any())
            {
                barco.BarcoHabitacion = dto.BarcoHabitacion.Select(bh => new BarcoHabitacion
                {
                    Idbarco = barco.Id,
                    Idhabitacion = bh.Idhabitacion,
                    Cantidad = bh.Cantidad
                }).ToList();
            }

            // Guardar en la base de datos
            return await _repository.AddAsync(barco);
        }
        public async Task<ICollection<HabitacionDTO>> FindByNameAsync(string nombre)
        {
            var list = await _repository.FindByNameAsync(nombre);

            var collection = _mapper.Map<ICollection<HabitacionDTO>>(list);

            return collection;

        }
        public async Task UpdateAsync(int id, BarcoDTO dto)
        {
            var barco = await _repository.FindByIdAsync(id);
            if (barco == null)
            {
                throw new Exception("El barco no existe.");
            }

            // Eliminar relaciones actuales en BarcoHabitacion
            await _repository.EliminarRelacionesHabitaciones(id);

            // Mapear DTO a entidad sin afectar la navegación de habitaciones
            _mapper.Map(dto, barco);

            // Si hay nuevas habitaciones, las agregamos
            if (dto.BarcoHabitacion != null && dto.BarcoHabitacion.Any())
            {
                barco.BarcoHabitacion = dto.BarcoHabitacion.Select(h => new BarcoHabitacion
                {
                    Idbarco = id,
                    Idhabitacion = h.Idhabitacion,
                    Cantidad = h.Cantidad
                }).ToList();
            }

            // Guardar cambios en la base de datos
            await _repository.UpdateAsync(barco);
        }

    }
}
