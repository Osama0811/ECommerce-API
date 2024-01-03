using CircuitsUc.Application.Communications;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.PageTypeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CircuitsUc.Application.Helpers.CommenEnum;

namespace CircuitsUc.Application.IServices
{
    public interface IPageTypeService
    {
        GeneralResponse<List<PageTypeDropDown>> PageTypeDDL(bool isEnglish);
      
    }
}
