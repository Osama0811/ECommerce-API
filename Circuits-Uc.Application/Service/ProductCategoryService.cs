using AutoMapper;
using CircuitsUc.Application.Common.SharedResources;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
using CircuitsUc.Application.DTOS.SecurityUserDTO;
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
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IDocumentService _documentService;
        private readonly IStringLocalizer<GeneralMessages> _localization;


        public ProductCategoryService(IUnitOfWork unitOfWork, IMapper Mapper, IStringLocalizer<GeneralMessages> localization, IDocumentService documentService)
        {
            _localization = localization;
            _mapper = Mapper;
            _unit = unitOfWork;
            _documentService = documentService;
        }
        public async Task<GeneralResponse<Guid>> Add(ProductCategoryInput Input, Guid UserId)
        {
            try
            {

                bool Succes = true;
                var ProductCategory = _mapper.Map<ProductCategoryInput, ProductCategory>(Input);
                ProductCategory.CreatedBy = UserId;
                ProductCategory.CreationDate = DateTime.Now;
                #region CheckProductCategory
                if (!CheckProductCategory(ProductCategory, out string message))
                {
                    return new GeneralResponse<Guid>(_localization[message].Value, System.Net.HttpStatusCode.BadRequest);
                }
                #endregion
                await _unit.ProductCategory.AddAsync(ProductCategory);
                #region Add IMG
                string _enumVal = Convert.ToInt32(CommenEnum.EntityType.ProductCategory).ToString();
              
                    #region Adding Doc
                    Document doc = new Document()
                    {
                        RefEntityTypeID = _enumVal,
                        RefEntityID = ProductCategory.Id.ToString(),
                        FileName = Input.FileName,
                        FileExtension = Path.GetExtension(Input.FileName),
                        IsMain = true,
                        CreatedBy = UserId
                    };
                    Succes = (await _documentService.Add(doc));
                    if (!Succes)
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                    }
                    #endregion

                    #region Saving  Image
                    _documentService.AddFileBase64(ProductCategory.Id, doc.Id, CommenEnum.EntityType.ProductCategory.ToString(), Input.FileName, Input.ImageBase64);

                #endregion



                #endregion
              

                return  new GeneralResponse<Guid>(ProductCategory.Id, _localization["AddedSuccessfully"].Value);

            }
            catch (Exception ex)
            {

                return new GeneralResponse<Guid>(ex.Message + "-" + ex.InnerException?.Message, System.Net.HttpStatusCode.BadRequest);

            }
        }

      

        public async Task<GeneralResponse<List<ProductCategoryDto>>> GetAll(Guid? ParentID, string? SearchTxt,bool IsEnglish)
        {
            var results = _unit.ProductCategory.All().Include(x=>x.Parent).ThenInclude(d=> d != null ? d.Parent : null)
                .Where(x=>(ParentID==null||x.ParentID==ParentID)
                && (SearchTxt == null || x.NameAr.Contains(SearchTxt) || x.NameEn.Contains(SearchTxt))).ToList()
                  .Select(x => new ProductCategoryDto
                  {
                      Id= x.Id,
                      Name=IsEnglish?x.NameEn:x.NameAr,
                      ParentName = x.Parent != null ? x.Parent.Parent != null ? IsEnglish ? $"{x.Parent.Parent.NameEn}/{x.Parent.NameEn}"  : $"{x.Parent.Parent.NameAr}/{x.Parent.NameAr}"
                      : IsEnglish ? x.Parent.NameEn : x.Parent.NameAr  : null,

                  }).ToList();
            return new GeneralResponse<List<ProductCategoryDto>>(results, results.Count().ToString());
        }

        public async Task<GeneralResponse<ProductCategoryDto>> GetByIdAsync(Guid Id,bool IsEnglish)
        {
            var results = _unit.ProductCategory.All().Include(x => x.Parent).ThenInclude(d=>d!=null? d.Parent:null)
                 .Where(x => x.Id == Id).
             ToList()
                   .Select(x => new ProductCategoryDto
                   {
                       Id = x.Id,
                       Name = IsEnglish ? x.NameEn : x.NameAr,
                       NameAr=x.NameAr,
                       NameEn=x.NameEn,
                       Icon = x.Icon,
                       Description = IsEnglish ? x.DescriptionEn : x.DescriptionAr,
                       DescriptionAr=x.DescriptionAr,
                       DescriptionEn=x.DescriptionEn,
                       ParentName = x.Parent != null ? x.Parent.Parent != null ? IsEnglish ? $"{x.Parent.Parent.NameEn}/{x.Parent.NameEn}" : $"{x.Parent.Parent.NameAr}/{x.Parent.NameAr}"
                      : IsEnglish ? x.Parent.NameEn : x.Parent.NameAr : null,
                       ImagePath = GetProductCategoryImage(Id)
                   }).FirstOrDefault();
            return new GeneralResponse<ProductCategoryDto>(results, _localization["Succes"].Value);
        }

        public async Task<GeneralResponse<Guid>> SoftDelete(Guid Id)
        {
           // var ProductCategory=_unit.ProductCategory.All().Where(x=>x.Id == Id).FirstOrDefault();
           
                var SubCategory=_unit.ProductCategory.All().Where(x=>x.ParentID==Id).Select(d=>d.Id).ToList();
            if ( SubCategory.Count != 0)
            {
                await SoftRangeDelete(SubCategory);
            }
                await _unit.ProductCategory.SoftDelete(Id);


                var Products = _unit.Product.All().Where(x => x.CategoryID == Id).Select(d => d.Id).ToList();
            if ( Products.Count != 0)
            {
                await _unit.Product.SoftDeleteRangeAsync(Products);
            }
         

            
            var results = await _unit.SaveAsync();

            return results >= 1 ? new GeneralResponse<Guid>(Id, _localization["DeletedSuccesfully"].Value) :
                new GeneralResponse<Guid>(_localization["ErrorInDelete"].Value, System.Net.HttpStatusCode.BadRequest);

        }

        public async Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> Id)
        {

            //await _unit.ProductCategory.SoftDeleteRangeAsync(Id);

            foreach(var Category in Id) {

                var SubCategory = _unit.ProductCategory.All().Where(x => x.ParentID == Category).Select(d => d.Id).ToList();
                if (SubCategory.Count != 0)
                {
                    await SoftRangeDelete(SubCategory);
                }
                await _unit.ProductCategory.SoftDelete(Category);


                var Products = _unit.Product.All().Where(x => x.CategoryID == Category).Select(d => d.Id).ToList();
                if (Products.Count != 0)
                {
                    await _unit.Product.SoftDeleteRangeAsync(Products);
                }
            }
            var results = _unit.Save();

            return results >= 1 ? new GeneralResponse<List<Guid>>(Id, _localization["DeletedSuccesfully"].Value) :
                 new GeneralResponse<List<Guid>>(_localization["ErrorInDelete"].Value, System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<GeneralResponse<Guid>> Update(ProductCategoryUpdateInput Input, Guid UserId)
        {
            try
            {

                ProductCategory category = await _unit.ProductCategory.GetByIdAsync(Input.Id);

                category = _mapper.Map<ProductCategoryUpdateInput, ProductCategory>(Input, category);
                category.UpdatedBy = UserId;
                category.UpdatedDate = DateTime.Now;
                #region 
                if (!CheckProductCategory(category, out string message))
                {
                    return new GeneralResponse<Guid>(_localization[message].Value, System.Net.HttpStatusCode.BadRequest);
                }


                #endregion

                #region Update IMG
                string _enumVal = Convert.ToInt32(CommenEnum.EntityType.ProductCategory).ToString();
                if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.FileName))
                {
                    #region Saving  Image
                    Document currentDoc = _documentService.GetMainDocumentByEntity(Input.Id.ToString(), _enumVal);

                    if (currentDoc != null)
                    {
                        await _documentService.Delete(currentDoc.Id);
                        _documentService.RemoveImage(Input.Id, currentDoc.Id, currentDoc.FileName, _enumVal);
                    }
                    #endregion


                    #region Adding Doc
                    Document doc = new Document()
                    {
                        RefEntityTypeID = _enumVal,
                        RefEntityID = Input.Id.ToString(),
                        FileName = Input.FileName,
                        FileExtension = Path.GetExtension(Input.FileName),
                        IsMain = true
                    };

                    if (!await _documentService.Add(doc))
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInUpdated"].Value, System.Net.HttpStatusCode.BadRequest);

                    }

                    _documentService.AddFileBase64(Input.Id, doc.Id, CommenEnum.EntityType.ProductCategory.ToString(), Input.FileName, Input.ImageBase64);





                    #endregion

                }


                #endregion

                await _unit.ProductCategory.UpdateAsync(category);
                var results = await _unit.SaveAsync();

                return results >= 1 ? new GeneralResponse<Guid>(category.Id, _localization["updatedSuccessfully"].Value) :
                    new GeneralResponse<Guid>(_localization["ErrorInEdit"].Value, System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return new GeneralResponse<Guid>(ex.Message + "-" + ex.InnerException?.Message, System.Net.HttpStatusCode.BadRequest);

            }
        }
        public async Task<GeneralResponse<List<DropDownResponse>>> GetProductCategoryDDL(bool isEnglish)
        {
            var data = _unit.ProductCategory.All()
                    .Select(z => new
                    {
                        ID = z.Id,
                        Title = isEnglish ? z.NameEn : z.NameAr,

                    });

            var results = await data.Select(z => new DropDownResponse
            {
                Id = z.ID,
                Name = z.Title,
            }).ToListAsync();

            return new GeneralResponse<List<DropDownResponse>>(results, results.Count().ToString());

        }

        public string? GetProductCategoryImage(Guid Id)
        {
            int _enumVal = (int)CommenEnum.EntityType.ProductCategory;
            Document currentDoc = _documentService.GetMainDocumentByEntity(Id.ToString(), _enumVal.ToString());
            if (currentDoc != null)
            {
                return _documentService.GetImagePath
                    (Id, currentDoc.Id,
                    CommenEnum.EntityType.ProductCategory.ToString(), currentDoc.FileName);
             
            }
            return null;
        }
        private bool CheckProductCategory(ProductCategory productCategory, out string message)
        {
            if (_unit.ProductCategory.All().Any(d => (d.NameEn == productCategory.NameEn || d.NameAr == productCategory.NameAr) && (productCategory.Id == Guid.Empty || productCategory.Id != d.Id)))
            {
                message = "ProductCategoryExistBefore";
                return false;
            }
            message = "Done";
            return true;
        }

        public async Task<GeneralResponse<List<ProductCategoryDto>>> GetAllCategoryPortal(Guid? ParentID, bool IsEnglish)
        {
            var results = _unit.ProductCategory.All().Include(x => x.Parent).ThenInclude(d => d.Parent)
               .Where(x => (ParentID == null || x.ParentID == ParentID)).ToList()
                 .Select(x => new ProductCategoryDto
                 {
                     Id = x.Id,
                     Name = IsEnglish ? x.NameEn : x.NameAr,
                     Icon = x.Icon,
                     Description = IsEnglish ? x.DescriptionEn : x.DescriptionAr,
                     ParentName = x.Parent != null ? x.Parent.Parent != null ? IsEnglish ? x.Parent.Parent.NameEn : x.Parent.Parent.NameAr
                      : x.Parent != null ? IsEnglish ? x.Parent.NameEn : x.Parent.NameAr : null : null,
                     ImagePath = GetProductCategoryImage(x.Id)
                 }).ToList();
            return new GeneralResponse<List<ProductCategoryDto>>(results, results.Count().ToString());
        }

        
    }
}
