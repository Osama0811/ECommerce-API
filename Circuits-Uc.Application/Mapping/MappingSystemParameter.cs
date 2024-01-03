using AutoMapper;
using CircuitsUc.Application.DTOS.SystemParameterDTO;
using CircuitsUc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Mapping
{
    public class MappingSystemParameter:Profile
    {
        public MappingSystemParameter()
        {

            CreateMap<SystemParameterInput, SystemParameter>();
            CreateMap<SystemParameterUpdateInput, SystemParameter>();
        }

    }
}
