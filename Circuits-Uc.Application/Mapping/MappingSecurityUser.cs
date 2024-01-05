using AutoMapper;
using CircuitsUc.Application.DTOS.SecurityUserDTO;
using CircuitsUc.Application.Models.AuthDTO;
using CircuitsUc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Mapping
{
    public class MappingSecurityUser : Profile
    {
        public MappingSecurityUser()
        {

            //CreateMap<UserInput, User>()
            // .ForMember(dest => dest.AlternativePhone, opt => opt.MapFrom(src => src.AlternativePhone))

            CreateMap<SecurityUserInput, SecurityUser>();
             
            CreateMap<SecurityUserUpdateInput, SecurityUser>()
                   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                   .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                   .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                   .ForMember(dest => dest.AlternativePhone, opt => opt.MapFrom(src => src.AlternativePhone))
                   .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            

        }
    }
    }
