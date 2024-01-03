using CircuitsUc.Api.Extentions;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.ProductDTO;
using CircuitsUc.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CircuitsUc.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _ProductService;
        public ProductController(IProductService ProductService)
        {

            _ProductService = ProductService;
        }
        [HttpGet("GetAll")]
        public async Task<GeneralResponse<List<ProductDto>>> GetAll(Guid? CategoryID, string? SearchTxt)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _ProductService.GetAll(CategoryID, SearchTxt, isEnglish);
        }
        [HttpGet("GetById")]
        public async Task<GeneralResponse<ProductDto>> GetById(Guid Id)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _ProductService.GetByIdAsync(Id, isEnglish);
        }



        [HttpPost("Add")]
        public async Task<GeneralResponse<Guid>> Add(ProductInput input)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _ProductService.Add(input, userId);
        }

        [HttpPost("Update")]
        public async Task<GeneralResponse<Guid>> Update(ProductUpdateInput input)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _ProductService.Update(input, userId);
        }
        [HttpPost("Delete")]
        public async Task<GeneralResponse<Guid>> Delete(Guid Id)
        {
            return await _ProductService.SoftDelete(Id);
        }
        [HttpPost("SoftRangeDelete")]
        public async Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> id)
        {
            return await _ProductService.SoftRangeDelete(id);
        }
    }
}
