using AutoMapper;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
using CircuitsUc.Application.DTOS.SecurityUserDTO;
using CircuitsUc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Mapping
{
    public class MappingProductCategory :Profile
    {
        public MappingProductCategory()
        {
            CreateMap<ProductCategoryInput, ProductCategory>();
            CreateMap<ProductCategoryUpdateInput, ProductCategory>()
                   .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                   .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameAr))
                   .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon))
                   .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr))
                   .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
                   .ForMember(dest => dest.ParentID, opt => opt.MapFrom(src => src.ParentID));
        }
    }
}
