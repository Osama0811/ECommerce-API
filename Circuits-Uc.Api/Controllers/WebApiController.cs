using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
using CircuitsUc.Application.DTOS.ProductDTO;
using CircuitsUc.Application.IService;
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
        public WebApiController(IProductCategoryService productCategoryService, IProductService productService) 
        {
            _ProductCategoryService = productCategoryService;
            _ProductService = productService;
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
        #endregion
    }
}
