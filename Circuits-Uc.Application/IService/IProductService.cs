using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
using CircuitsUc.Application.DTOS.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.IService
{
    public interface IProductService
    {
        Task<GeneralResponse<List<ProductDto>>> GetAll(Guid? CategoryID, string? SearchTxt, bool IsEnglish);
        //Task<GeneralResponse<List<ProductDto>>> GetAllProductPortal(Guid? CategoryID, bool IsEnglish);
        Task<GeneralResponse<ProductDto>> GetByIdAsync(Guid Id, bool IsEnglish);
        Task<GeneralResponse<Guid>> Add(ProductInput Input, Guid UserId);
        Task<GeneralResponse<Guid>> Update(ProductUpdateInput Input, Guid UserId);
        Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id);
        Task<GeneralResponse<Guid>> SoftDelete(Guid Id);
    }
}
