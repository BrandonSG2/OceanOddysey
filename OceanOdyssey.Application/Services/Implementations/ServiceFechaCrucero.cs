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
    public class ServiceFechaCrucero : IServiceFechaCrucero
    {


        private readonly IRepositoryFechaCrucero _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceHabitacion> _logger;

        public ServiceFechaCrucero(IRepositoryFechaCrucero repository, IMapper mapper, ILogger<ServiceHabitacion> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<FechaCruceroDTO>> FechaXCrucero(int idCrucero)
        {
            var fechas = await _repository.FechaXCrucero(idCrucero);



            var fechasDTO = _mapper.Map<ICollection<FechaCruceroDTO>>(fechas);

            return fechasDTO;
        }

        public async Task<ICollection<PrecioHabitacionDTO>> PreciosHabitacionesPorFecha(int idCrucero)
        {
            var fechas = await _repository.PreciosHabitacionesPorFecha(idCrucero);



            var fechasDTO = _mapper.Map<ICollection<PrecioHabitacionDTO>>(fechas);

            return fechasDTO;
        }

        public async Task<FechaCruceroDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<FechaCruceroDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<FechaCruceroDTO>> ListAsync()
        {
            //obtener datos del repositorio 

            var list = await _repository.ListAsync();
            // map List<Barco> a ICollection<BarcoDTO>
            var collection = _mapper.Map<ICollection<FechaCruceroDTO>>(list);
            // retorna la lista
            return collection;
        }
    }
}
