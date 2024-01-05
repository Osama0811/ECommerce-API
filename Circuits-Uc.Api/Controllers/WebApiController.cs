using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.PageContentDTO;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
using CircuitsUc.Application.DTOS.ProductDTO;
using CircuitsUc.Application.IService;
using CircuitsUc.Application.IServices;
using CircuitsUc.Application.Service;
using CircuitsUc.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CircuitsUc.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebApiController : ControllerBase
    {

        private readonly IProductCategoryService _ProductCategoryService;
        private readonly IProductService _ProductService;
        private readonly ISystemParameterServices _SystemParameter;
        private readonly IPageContentService _pageContentService;

        public WebApiController(IProductCategoryService productCategoryService, IProductService productService, ISystemParameterServices SystemParameter, IPageContentService PageContentService) 
        {
            _ProductCategoryService = productCategoryService;
            _ProductService = productService;
            _SystemParameter = SystemParameter;
            _pageContentService = PageContentService;
        }
        #region ProductCategory

        [HttpGet("ProductCategory/GetAll")]
        public async Task<GeneralResponse<List<ProductCategoryDto>>> GetAllProductCategory(Guid? ParentID)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _ProductCategoryService.GetAllCategoryPortal(ParentID, isEnglish);
        }
        #endregion
        #region Product
        [HttpGet("Product/GetAll")]
        public async Task<GeneralResponse<List<ProductDto>>> GetAllProduct(Guid? CategoryID)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _ProductService.GetAll(CategoryID, null, isEnglish);
        }
        [HttpGet("Product/ProductDetails")]
        public async Task<GeneralResponse<ProductDto>> ProductDetails(Guid Id)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _ProductService.GetByIdAsync(Id, isEnglish);
        }
        #endregion
        #region SystemParameter
        #endregion

        #region PageContent
        [HttpGet("PageContent/GetAll")]
        public async Task<GeneralResponse<List<PageContentDto>>> GetAllPageContent(int? TypeID,int? Count)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _pageContentService.GetAll(TypeID, Count, isEnglish);
        }
        [HttpGet("PageContent/PageContentDetails")]
        public async Task<GeneralResponse<PageContentDto>> PageContentDetails(Guid Id)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _pageContentService.GetByIdAsync(Id, isEnglish);
        }
        #endregion
    }
}
