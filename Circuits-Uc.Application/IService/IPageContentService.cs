using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.PageContentDTO;
using CircuitsUc.Application.DTOS.ProductDTO;
using CircuitsUc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.IService
{
    public interface IPageContentService
    {
        Task<GeneralResponse<List<PageContentDto>>> GetAll(int? TypeID,int? Count ,bool IsEnglish);
        Task<GeneralResponse<PageContentDto>> GetByIdAsync(Guid Id, bool IsEnglish);
        Task<GeneralResponse<Guid>> Add(PageContentInput Input, Guid UserId);
        Task<GeneralResponse<Guid>> Update(PageContentUpdateInput Input, Guid UserId);
        Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id);
        Task<GeneralResponse<Guid>> SoftDelete(Guid Id);
    }
}
