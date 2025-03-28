﻿using OceanOdyssey.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanOdyssey.Application.Services.Interfaces
{
    public interface IServiceFechaCrucero
    {
        Task<ICollection<FechaCruceroDTO>> ListAsync();
    }
}
