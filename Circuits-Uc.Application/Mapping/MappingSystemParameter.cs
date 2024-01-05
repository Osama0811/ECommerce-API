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
            CreateMap<SystemParameterUpdateInput, SystemParameter>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.SettingValueAR, opt => opt.MapFrom(src => src.SettingValueAR))
                  .ForMember(dest => dest.SettingValueEN, opt => opt.MapFrom(src => src.SettingValueEN))
                  .ForMember(dest => dest.SettingKey, opt => opt.MapFrom(src => src.SettingKey))
                  .ForMember(dest => dest.IsSystemKey, opt => opt.MapFrom(src => src.IsSystemKey)); 

            //   .ForMember(dest => dest.SettingValueAR, opt => opt.MapFrom(src => src.SettingValueAR ?? GetOldValueFromDatabase(src.Id, "SettingValueAR")))
        }

    }
}
