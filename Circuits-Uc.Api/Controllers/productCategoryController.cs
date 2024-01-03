using CircuitsUc.Api.Extentions;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.ProductCategoryDTO;
using CircuitsUc.Application.DTOS.SecurityUserDTO;
using CircuitsUc.Application.IService;
using CircuitsUc.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CircuitsUc.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productCategoryController :  BaseApiController
    {
        private readonly IProductCategoryService _ProductCategoryService;
        public productCategoryController(IProductCategoryService ProductCategoryService)
        {

            _ProductCategoryService = ProductCategoryService;
        }
        [HttpGet("GetAll")]
        public async Task<GeneralResponse<List<ProductCategoryDto>>> GetAll(Guid? ParentID, string? SearchTxt)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _ProductCategoryService.GetAll(ParentID,SearchTxt,isEnglish);
        }
        [HttpGet("GetById")]
        public async Task<GeneralResponse<ProductCategoryDto>> GetById(Guid Id)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _ProductCategoryService.GetByIdAsync(Id,isEnglish);
        }



        [HttpPost("Add")]
        public async Task<GeneralResponse<Guid>> Add(ProductCategoryInput input)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _ProductCategoryService.Add(input, userId);
        }

        [HttpPost("Update")]
        public async Task<GeneralResponse<Guid>> Update(ProductCategoryUpdateInput input)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _ProductCategoryService.Update(input, userId);
        }
        [HttpPost("Delete")]
        public async Task<GeneralResponse<Guid>> Delete(Guid Id)
        {
            return await _ProductCategoryService.SoftDelete(Id);
        }
        [HttpPost("SoftRangeDelete")]
        public async Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> id)
        {
            return await _ProductCategoryService.SoftRangeDelete(id);
        }
    }
}
