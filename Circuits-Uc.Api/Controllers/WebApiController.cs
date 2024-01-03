using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
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
        public WebApiController(IProductCategoryService productCategoryService) 
        {
            _ProductCategoryService = productCategoryService;
        }

        [HttpGet("ProductCategory/GetAll")]
        public async Task<GeneralResponse<List<ProductCategoryDto>>> GetAll(Guid? ParentID, string? SearchTxt)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _ProductCategoryService.GetAllCategoryPortal(ParentID, isEnglish);
        }
    }
}
