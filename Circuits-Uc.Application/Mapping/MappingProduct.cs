using AutoMapper;
using CircuitsUc.Application.DTOS.ProductDTO;
using CircuitsUc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Mapping
{
    public class MappingProduct:Profile
    {
        public MappingProduct()
        {
            CreateMap<ProductInput, Product>()
            .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr != null ? src.DescriptionAr : src.ShortDescriptionAr))
            .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn != null ? src.DescriptionEn : src.ShortDescriptionEn));
            
            CreateMap<ProductUpdateInput, Product>()
            .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr != null ? src.DescriptionAr : src.ShortDescriptionAr))
            .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn != null ? src.DescriptionEn : src.ShortDescriptionEn));
        }

    }
}
