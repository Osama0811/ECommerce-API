using AutoMapper;
using CircuitsUc.Application.Common.SharedResources;
using CircuitsUc.Application.Communications;
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
    public class ProductService:IProductService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IDocumentService _documentService;
        private readonly IStringLocalizer<GeneralMessages> _localization;


        public ProductService(IUnitOfWork unitOfWork, IMapper Mapper, IStringLocalizer<GeneralMessages> localization, IDocumentService documentService)
        {
            _localization = localization;
            _mapper = Mapper;
            _unit = unitOfWork;
            _documentService = documentService;
        }
        public async Task<GeneralResponse<Guid>> Add(ProductInput Input, Guid UserId)
        {
            try
            {

                var Product = _mapper.Map<ProductInput, Product>(Input);
                Product.CreatedBy = UserId;
                Product.CreationDate = DateTime.Now;
                Product.DescriptionAr = Input.DescriptionAr != null ? Input.DescriptionAr.ToString() : Input.ShortDescriptionAr;
                Product.DescriptionEn = Input.DescriptionEn != null ? Input.DescriptionEn.ToString() : Input.ShortDescriptionEn;
                #region CheckProduct
                if (!CheckProduct(Product, out string message))
                {
                    return new GeneralResponse<Guid>(_localization[message].Value, System.Net.HttpStatusCode.BadRequest);
                }
                #endregion
                await _unit.Product.AddAsync(Product);
                #region UpLoad File & Image
                if (!string.IsNullOrEmpty(Input.FileBase64) && !string.IsNullOrEmpty(Input.FileName))
                {
                    if (! await _documentService.PostImageAction(Product.Id, Convert.ToInt32(CommenEnum.EntityType.ProductFile).ToString(), Input.FileBase64,Input.FileName,false))
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                    }
                }
             //Image
                    if (!await _documentService.PostImageAction(Product.Id, Convert.ToInt32(CommenEnum.EntityType.ProductImage).ToString(), Input.ImageBase64, Input.ImageName, true))
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                    }
                
                #endregion
                //#region Add File
                //if (!string.IsNullOrEmpty(Input.FileBase64) && !string.IsNullOrEmpty(Input.FileName))
                //{
                //    string _FileenumVal = CommenEnum.EntityType.ProductFile.ToString();

                //    #region Adding Doc
                //    Document FileDocument = new Document()
                //    {
                //        RefEntityTypeID = _FileenumVal,
                //        RefEntityID = Product.Id.ToString(),
                //        FileName = Input.FileName,
                //        FileExtension = Path.GetExtension(Input.FileName),
                //        IsMain = false,
                //        CreatedBy = UserId
                //    };
                //    Succes = (await _documentService.Add(FileDocument));
                //    if (!Succes)
                //    {

                //        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                //    }
                //    #endregion

                //    #region Saving  Image
                //    _documentService.AddFileBase64(Product.Id, FileDocument.Id, _FileenumVal, Input.FileName, Input.FileBase64);
                //}
                //#endregion

                //Succes = _unit.Save() >= 1 ? true : false;

                //#endregion
                //#region Add IMG
                //string _ImgenumVal = CommenEnum.EntityType.ProductImage.ToString();

                //    #region Adding Doc
                //    Document ImageDocument = new Document()
                //    {
                //        RefEntityTypeID = _ImgenumVal,
                //        RefEntityID = Product.Id.ToString(),
                //        FileName = Input.ImageName,
                //        FileExtension = Path.GetExtension(Input.ImageName),
                //        IsMain = true,
                //        CreatedBy = UserId
                //    };
                //    Succes = (await _documentService.Add(ImageDocument));
                //    if (!Succes)
                //    {

                //        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                //    }
                //    #endregion

                //    #region Saving  Image
                //    _documentService.AddFileBase64(Product.Id, ImageDocument.Id, _ImgenumVal, Input.ImageName, Input.ImageBase64);

                //#endregion


                //#endregion
                //var results =Succes? 1: 0;

                return  new GeneralResponse<Guid>(Product.Id, _localization["AddedSuccessfully"].Value) ;

            }
            catch (Exception ex)
            {

                return new GeneralResponse<Guid>(ex.Message + "-" + ex.InnerException?.Message, System.Net.HttpStatusCode.BadRequest);

            }
        }



        public async Task<GeneralResponse<List<ProductDto>>> GetAll(Guid? CategoryID, string? SearchTxt, bool IsEnglish)
        {
            var results = _unit.Product.All().Include(x => x.Category).ThenInclude(d => d != null ? d.Parent : null)
                .Where(x => (CategoryID == null || x.CategoryID == CategoryID)
                && (SearchTxt == null || x.NameAr.Contains(SearchTxt) || x.NameEn.Contains(SearchTxt))).ToList()
                  .Select(x => new ProductDto
                  {
                      Id = x.Id,
                      Name = IsEnglish ? x.NameEn : x.NameAr,
                      ShortDescription=IsEnglish? x.ShortDescriptionEn : x.ShortDescriptionAr,
                      //       CategoryName = (IsEnglish ? x?.Category?.Parent?.NameEn : x?.Category?.Parent?.NameAr) ??
                      //(IsEnglish ? x?.Category?.NameEn : x?.Category?.NameAr) ?? null,
                      CategoryName = x.Category != null ? x.Category.Parent != null ? IsEnglish ? $"{x.Category.Parent.NameEn}/{x.Category.NameEn}" : $"{x.Category.Parent.NameEn}/{x.Category.NameEn}"
                      : x.Category != null ? IsEnglish ? x.Category.NameEn : x.Category.NameAr : null : null,

                  }).ToList();
            return new GeneralResponse<List<ProductDto>>(results, results.Count().ToString());
        }
       

        public async Task<GeneralResponse<ProductDto>> GetByIdAsync(Guid Id, bool IsEnglish)
        {
            var results = _unit.Product.All().Include(x => x.Category).ThenInclude(d => d != null ? d.Parent : null)
                 .Where(x => x.Id == Id).ToList()
                   .Select(x => new ProductDto
                   {
                       Id = x.Id,
                       Name = IsEnglish ? x.NameEn : x.NameAr,
                       NameAr = x.NameAr,
                       NameEn = x.NameEn,
                       ShortDescription = IsEnglish ? x.ShortDescriptionEn : x.ShortDescriptionAr,
                       ShortDescriptionAr = x.ShortDescriptionAr,
                       ShortDescriptionEn = x.ShortDescriptionEn,
                       Description = IsEnglish ? x.DescriptionEn : x.DescriptionAr,
                       DescriptionEn = x.DescriptionEn,
                       DescriptionAr = x.DescriptionAr,
                       CategoryName = x.Category != null ? x.Category.Parent != null ? IsEnglish ? $"{x.Category.NameEn}/{x.Category.Parent.NameEn}" : $"{x.Category.NameEn}/{x.Category.Parent.NameEn}"
                      : x.Category != null ? IsEnglish ? x.Category.NameEn : x.Category.NameAr : null : null,
                       //CategoryName=x.Category!.Parent.NameAr,
                       // CategoryName = x.Category != null ? x.Category.Parent != null ? IsEnglish ? x.Category.Parent.NameEn : x.Category.Parent.NameAr
                       //: x.Category != null ? IsEnglish ? x.Category.NameEn : x.Category.NameAr : null : null,
                       ImagePath = GetProductImage(Id),
            FilePath = GetProductFile(Id),
                   }).FirstOrDefault();
            return new GeneralResponse<ProductDto>(results, _localization["Succes"].Value);
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

        public async Task<GeneralResponse<Guid>> Update(ProductUpdateInput Input, Guid UserId)
        {
            try
            {

                Product product  = await _unit.Product.GetByIdAsync(Input.Id);

                product = _mapper.Map<ProductUpdateInput, Product>(Input, product);
                product.UpdatedBy = UserId;
                product.UpdatedDate = DateTime.Now;
                #region 
                if (!CheckProduct(product, out string message))
                {
                    return new GeneralResponse<Guid>(_localization[message].Value, System.Net.HttpStatusCode.BadRequest);
                }
                #endregion
                #region UpLoad File & Image
                if (!string.IsNullOrEmpty(Input.FileBase64) && !string.IsNullOrEmpty(Input.FileName))
                {
                    if (!await _documentService.PostImageAction(product.Id, Convert.ToInt32(CommenEnum.EntityType.ProductFile).ToString(), Input.FileBase64, Input.FileName, false))
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                    }
                }
                if (!string.IsNullOrEmpty(Input.ImageName) && !string.IsNullOrEmpty(Input.ImageName))
                {
                    if (!await _documentService.PostImageAction(product.Id, Convert.ToInt32(CommenEnum.EntityType.ProductImage).ToString(), Input.ImageBase64, Input.ImageName, true))
                    {

                        return new GeneralResponse<Guid>(_localization["ErrorInSave"].Value, System.Net.HttpStatusCode.BadRequest);

                    }
                }
                #endregion
                //#region Update File
                //string _FileeEnumVal = CommenEnum.EntityType.ProductFile.ToString();
                //if (!string.IsNullOrEmpty(Input.FileBase64) && !string.IsNullOrEmpty(Input.FileName))
                //{
                //    #region Saving  Image
                //    Document currentDoc = _documentService.GetMainDocumentByEntity(Input.Id.ToString(), _FileeEnumVal);

                //    if (currentDoc != null)
                //    {
                //        await _documentService.Delete(currentDoc.Id);
                //        _documentService.RemoveImage(Input.Id, currentDoc.Id, currentDoc.FileName, _FileeEnumVal);
                //    }
                //    #endregion


                //    #region Adding Doc
                //    Document doc = new Document()
                //    {
                //        RefEntityTypeID = _FileeEnumVal,
                //        RefEntityID = Input.Id.ToString(),
                //        FileName = Input.FileName,
                //        FileExtension = Path.GetExtension(Input.FileName),
                //        IsMain = false
                //    };

                //    if (!await _documentService.Add(doc))
                //    {

                //        return new GeneralResponse<Guid>(_localization["ErrorInUpdated"].Value, System.Net.HttpStatusCode.BadRequest);

                //    }

                //    _documentService.AddFileBase64(Input.Id, doc.Id, _FileeEnumVal, Input.FileName, Input.FileBase64);





                //    #endregion

                //}


                //#endregion
                //#region Update IMG
                //string _enumVal = CommenEnum.EntityType.ProductImage.ToString();
                //if (!string.IsNullOrEmpty(Input.ImageBase64) && !string.IsNullOrEmpty(Input.ImageName))
                //{
                //    #region Saving  Image
                //    Document currentDoc = _documentService.GetMainDocumentByEntity(Input.Id.ToString(), _enumVal);

                //    if (currentDoc != null)
                //    {
                //        await _documentService.Delete(currentDoc.Id);
                //        _documentService.RemoveImage(Input.Id, currentDoc.Id, currentDoc.FileName, _enumVal);
                //    }
                //    #endregion


                //    #region Adding Doc
                //    Document doc = new Document()
                //    {
                //        RefEntityTypeID = _enumVal,
                //        RefEntityID = Input.Id.ToString(),
                //        FileName = Input.ImageName,
                //        FileExtension = Path.GetExtension(Input.ImageName),
                //        IsMain = true
                //    };

                //    if (!await _documentService.Add(doc))
                //    {

                //        return new GeneralResponse<Guid>(_localization["ErrorInUpdated"].Value, System.Net.HttpStatusCode.BadRequest);

                //    }

                //    _documentService.AddFileBase64(Input.Id, doc.Id, _enumVal, Input.ImageName, Input.ImageBase64);





                //    #endregion

                //}


                //#endregion

                await _unit.Product.UpdateAsync(product);
                var results = await _unit.SaveAsync();

                return results >= 1 ? new GeneralResponse<Guid>(product.Id, _localization["updatedSuccessfully"].Value) :
                    new GeneralResponse<Guid>(_localization["ErrorInEdit"].Value, System.Net.HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return new GeneralResponse<Guid>(ex.Message + "-" + ex.InnerException?.Message, System.Net.HttpStatusCode.BadRequest);

            }
        }
        public string? GetProductImage(Guid Id)
        {
            int _enumVal = (int)CommenEnum.EntityType.ProductImage;
            Document currentDoc = _documentService.GetMainDocumentByEntity(Id.ToString(), _enumVal.ToString());
            if (currentDoc != null)
            {
                return _documentService.GetImagePath
                    (Id, currentDoc.Id,
                     CommenEnum.EntityType.ProductImage.ToString(), currentDoc.FileName);

            }
            return null;
        } 
        public string? GetProductFile(Guid Id)
        {
            int _enumVal = (int)CommenEnum.EntityType.ProductFile;
            Document currentDoc = _documentService.GetDocumentByEntity(Id.ToString(), _enumVal.ToString()).LastOrDefault();
            if (currentDoc != null)
            {
                return _documentService.GetImagePath
                    (Id, currentDoc.Id,
                    CommenEnum.EntityType.ProductFile.ToString(), currentDoc.FileName);

            }
            return null;
        }
        private bool CheckProduct(Product product, out string message)
        {
            if (_unit.Product.All().Any(d => (d.NameEn == product.NameEn || d.NameAr == product.NameAr) && (d.CategoryID==product.CategoryID) &&(product.Id == Guid.Empty || product.Id != d.Id)))
            {
                message = "ProductExistBefore";
                return false;
            }
            message = "Done";
            return true;
        }

        //public async Task<GeneralResponse<List<ProductDto>>> GetAllPortal(Guid? ParentID, bool IsEnglish)
        //{
        //    var results = _unit.Product.All().Include(x => x.Parent).ThenInclude(d => d.Parent)
        //       .Where(x => (ParentID == null || x.ParentID == ParentID)).ToList()
        //         .Select(x => new ProductDto
        //         {
        //             Id = x.Id,
        //             Name = IsEnglish ? x.NameEn : x.NameAr,
        //             Icon = x.Icon,
        //             Description = IsEnglish ? x.DescriptionEn : x.DescriptionAr,
        //             ParentName = x.Parent != null ? x.Parent.Parent != null ? IsEnglish ? x.Parent.Parent.NameEn : x.Parent.Parent.NameAr
        //              : x.Parent != null ? IsEnglish ? x.Parent.NameEn : x.Parent.NameAr : null : null,
        //             ImagePath = GetProductImage(x.Id)
        //         }).ToList();
        //    return new GeneralResponse<List<ProductDto>>(results, results.Count().ToString());
        //}


    }
}
