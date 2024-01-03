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
            CreateMap<ProductCategoryUpdateInput, ProductCategory>();
        }
    }
}
