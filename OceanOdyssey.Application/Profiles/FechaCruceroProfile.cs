using AutoMapper;
using OceanOdyssey.Application.DTOs;
using OceanOdyssey.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Profiles
{
    public class FechaCruceroProfile : Profile
    {
        public FechaCruceroProfile()
        {

            CreateMap<FechaCruceroDTO, FechaCrucero>().ReverseMap();
        }
    }
}
