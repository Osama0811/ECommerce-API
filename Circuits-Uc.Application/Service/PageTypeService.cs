using AutoMapper;
using CircuitsUc.Application.Common.SharedResources;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.PageTypeDTO;
using CircuitsUc.Application.Helpers;
using CircuitsUc.Application.IServices;
using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CircuitsUc.Application.Helpers.CommenEnum;

namespace CircuitsUc.Application.Services
{
    public class PageTypeService : IPageTypeService
    {

        private readonly IStringLocalizer<GeneralMessages> _localization;

        public PageTypeService(IStringLocalizer<GeneralMessages> localization)
        {
            _localization = localization;
        }

      
        public GeneralResponse<List<PageTypeDropDown>> PageTypeDDL(bool isEnglish)
        {
           
           

           var results= Enum.GetValues(typeof(PageType))
                         .Cast<PageType>()
                         .Select(x => new PageTypeDropDown
                         { 
                             TypeId = Convert.ToInt32(x),
                             PageName = _localization[x.ToString()].Value
                         })
                         .ToList();
           
           
              

            return new GeneralResponse<List<PageTypeDropDown>>(results, _localization["Succes"].Value, results.Count());

        }
      
     
    }
}