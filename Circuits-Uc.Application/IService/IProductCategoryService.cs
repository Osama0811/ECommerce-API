using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
using CircuitsUc.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.IService
{
    public interface IProductCategoryService
    {
        Task<GeneralResponse<List<ProductCategoryDto>>> GetAll(Guid? ParentID,string? SearchTxt,bool IsMain,bool NeedImg ,bool IsEnglish);
     
        Task<GeneralResponse<List<DropDownResponse>>> GetProductCategoryDDL(bool isEnglish);
        Task<GeneralResponse<ProductCategoryDto>> GetByIdAsync(Guid Id,bool IsEnglish);
        Task<GeneralResponse<Guid>> Add(ProductCategoryInput Input, Guid UserId);
        Task<GeneralResponse<Guid>> Update(ProductCategoryUpdateInput Input, Guid UserId);
        Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id);
        Task<GeneralResponse<Guid>> SoftDelete(Guid Id);
    }
}
