using AutoMapper;
using CircuitsUc.Application.DTOS.PageContentDTO;
using CircuitsUc.Application.DTOS.ProductDTO;
using CircuitsUc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Mapping
{
    public class MappingPageContent : Profile
    {
        public MappingPageContent()
        {
            CreateMap<PageContentInput, PageContent>()
            .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr != null ? src.DescriptionAr : src.ShortDescriptionAr))
            .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn != null ? src.DescriptionEn : src.ShortDescriptionEn));

            CreateMap<PageContentUpdateInput, PageContent>()
                   .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr != null ? src.DescriptionAr : src.ShortDescriptionAr))
                   .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn != null ? src.DescriptionEn : src.ShortDescriptionEn));
        }
    }
}
