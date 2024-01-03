using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.IService
{
    public interface IProductCategoryService
    {
        Task<GeneralResponse<List<ProductCategoryDto>>> GetAll(Guid? ParentID,string? SearchTxt,bool IsEnglish);
        Task<GeneralResponse<List<ProductCategoryDto>>> GetAllCategoryPortal(Guid? ParentID,bool IsEnglish);
        Task<GeneralResponse<ProductCategoryDto>> GetByIdAsync(Guid Id,bool IsEnglish);
        Task<GeneralResponse<Guid>> Add(ProductCategoryInput Input, Guid UserId);
        Task<GeneralResponse<Guid>> Update(ProductCategoryUpdateInput Input, Guid UserId);
        Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id);
        Task<GeneralResponse<Guid>> SoftDelete(Guid Id);
    }
}
