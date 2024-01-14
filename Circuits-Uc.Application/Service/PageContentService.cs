using AutoMapper;
using CircuitsUc.Application.Common.SharedResources;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.PageContentDTO;
using CircuitsUc.Application.DTOS.ProductDTO;
using CircuitsUc.Application.Helpers;
using CircuitsUc.Application.IService;
using CircuitsUc.Application.IServices;
using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Service
{
    public class PageContentService : IPageContentService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IDocumentService _documentService;
        private readonly IStringLocalizer<GeneralMessages> _localization;


        public PageContentService(IUnitOfWork unitOfWork, IMapper Mapper, IStringLocalizer<GeneralMessages> localization, IDocumentService documentService)
        {
            _localization = localization;
            _mapper = Mapper;
            _unit = unitOfWork;
            _documentService = documentService;
        }
        public async Task<GeneralResponse<Guid>> Add(PageContentInput Input, Guid UserId)
        {
            try
            {

                var PageContent = _mapper.Map<PageContentInput, PageContent>(Input);
                PageContent.CreatedBy = UserId;
                PageContent.CreationDate = DateTime.Now;
                #region CheckProduct
                if (!CheckPageContent(PageContent, out string message))
                {
                    return new GeneralResponse<Guid>(_localization[message].Value, System.Net.HttpStatusCode.BadRequest);
                }
                #endregion
                await _unit.PageContent.AddAsync(PageContent);

                if(_unit.Save()<=0)
                    return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                #region UpLoad File & Image
                if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.FileName))
                {
                    if (!await _documentService.PostImageAction(PageContent.Id, Convert.ToInt32(CommenEnum.EntityType.PageContent).ToString(), Input.ImageBase64, Input.FileName, false))
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                    }
                }
               
                #endregion
               

                return new GeneralResponse<Guid>(PageContent.Id, _localization["AddedSuccessfully"].Value);

            }
            catch (Exception ex)
            {

                return new GeneralResponse<Guid>(ex.Message + "-" + ex.InnerException?.Message, System.Net.HttpStatusCode.BadRequest);

            }
        }

        public async Task<GeneralResponse<List<PageContentDto>>> GetAll(string? PageType,int? Count ,bool IsEnglish)
        {
            int TypeID = 0;
            if (PageType != null)
            {
                TypeID = Convert.ToInt32( Enum.Parse(typeof(CommenEnum.PageType), PageType));
            }
            var results = _unit.PageContent.All();
           var result= results.Where(x => (PageType == null || x.TypeID == TypeID)).ToList()
                  .Select(x => new PageContentDto
                  {
                      Id = x.Id,
                      Name = IsEnglish ? x.NameEn : x.NameAr,
                      ShortDescription = IsEnglish ? x.ShortDescriptionEn : x.ShortDescriptionAr,
                      PostedBy = x.PostedBy,
                      PostedDate = x.PostedDate!=null?x.PostedDate.Value.Date.ToString() :null,//MM/dd/yyyy
                      TypeID = x.TypeID,
                      TypeName=  Enum.GetName(typeof(CommenEnum.PageType),x.TypeID),
                      ImagePath= GetPageContentImage(x.Id)

                  }).OrderByDescending(d=>d.PostedDate).ThenBy(x=>x.Rank).Take(Count!=null?(int)Count:results.Count()).ToList();
            return new GeneralResponse<List<PageContentDto>>(result, result.Count().ToString());
        }

        public async Task<GeneralResponse<PageContentDto>> GetByIdAsync(Guid Id, bool IsEnglish)
        {
            var results = _unit.PageContent.All()
               .Where(x => ( x.Id == Id)).ToList()
                 .Select(x => new PageContentDto
                 {
                     Id = x.Id,
                     NameAr=x.NameAr,
                     NameEn=x.NameEn,
                     Name = IsEnglish ? x.NameEn : x.NameAr,
                     ShortDescription = IsEnglish ? x.ShortDescriptionEn : x.ShortDescriptionAr,
                     ShortDescriptionAr=x.ShortDescriptionAr,
                     ShortDescriptionEn=x.ShortDescriptionEn,
                     Description = IsEnglish ? x.DescriptionEn : x.DescriptionAr,
                     DescriptionAr=x.DescriptionAr,
                     DescriptionEn=x.DescriptionEn,
                     PostedBy = x.PostedBy,
                     PostedDate = x.PostedDate != null ? x.PostedDate.Value.Date.ToString() : null,//MM/dd/yyyy
                     TypeID = x.TypeID,
                     TypeName = Enum.GetName(typeof(CommenEnum.PageType), x.TypeID),
                     ImagePath = GetPageContentImage(x.Id)

                 }).FirstOrDefault();
            return new GeneralResponse<PageContentDto>(results, _localization["Succes"].Value);
        }

        public async Task<GeneralResponse<Guid>> SoftDelete(Guid Id)
        {
            await _unit.Product.SoftDelete(Id);
            var results = await _unit.SaveAsync();

            return results >= 1 ? new GeneralResponse<Guid>(Id, _localization["DeletedSuccesfully"].Value) :
                new GeneralResponse<Guid>(_localization["ErrorInDelete"].Value, System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id)
        {
            await _unit.Product.SoftDeleteRangeAsync(Id);

            var results = _unit.Save();

            return results >= 1 ? new GeneralResponse<List<Guid>>(Id, _localization["DeletedSuccesfully"].Value) :
                 new GeneralResponse<List<Guid>>(_localization["ErrorInDelete"].Value, System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<GeneralResponse<Guid>> Update(PageContentUpdateInput Input, Guid UserId)
        {
            try
            {

                PageContent pageContent = await _unit.PageContent.GetByIdAsync(Input.Id);

                pageContent = _mapper.Map<PageContentUpdateInput, PageContent>(Input, pageContent);
                pageContent.UpdatedBy = UserId;
                pageContent.UpdatedDate = DateTime.Now;
                #region 
                if (!CheckPageContent(pageContent, out string message))
                {
                    return new GeneralResponse<Guid>(_localization[message].Value, System.Net.HttpStatusCode.BadRequest);
                }
                #endregion
                #region UpLoad File & Image
                if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.FileName))
                {
                    if (!await _documentService.PostImageAction(pageContent.Id, Convert.ToInt32(CommenEnum.EntityType.PageContent).ToString(), Input.ImageBase64, Input.FileName, false))
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                    }
                }
               
                #endregion
             
                await _unit.PageContent.UpdateAsync(pageContent);
                var results = await _unit.SaveAsync();

                return results >= 1 ? new GeneralResponse<Guid>(pageContent.Id, _localization["updatedSuccessfully"].Value) :
                    new GeneralResponse<Guid>(_localization["ErrorInEdit"].Value, System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return new GeneralResponse<Guid>(ex.Message + "-" + ex.InnerException?.Message, System.Net.HttpStatusCode.BadRequest);

            }
        }
        public string? GetPageContentImage(Guid Id)
        {
            int _enumVal = (int)CommenEnum.EntityType.PageContent;
            Document currentDoc = _documentService.GetMainDocumentByEntity(Id.ToString(), _enumVal.ToString());
            if (currentDoc != null)
            {
                return _documentService.GetImagePath
                    (Id, currentDoc.Id,
                     CommenEnum.EntityType.PageContent.ToString(), currentDoc.FileName);

            }
            return null;
        }
        private bool CheckPageContent(PageContent pageContent, out string message)
        {
            if (_unit.PageContent.All().Any(d => (d.NameEn == pageContent.NameEn || d.NameAr == pageContent.NameAr) && (pageContent.Id == Guid.Empty || pageContent.Id != d.Id)))
            {
                message = "PageContentExistBefore";
                return false;
            }
            message = "Done";
            return true;
        }
    }
}
