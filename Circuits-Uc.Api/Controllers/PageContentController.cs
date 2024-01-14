using CircuitsUc.Api.Extentions;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.PageContentDTO;
using CircuitsUc.Application.DTOS.ProductDTO;
using CircuitsUc.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CircuitsUc.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageContentController : BaseApiController
    {
        private readonly IPageContentService _pageContentService;
        public PageContentController(IPageContentService PageContentService)
        {

            _pageContentService = PageContentService;
        }
        [HttpGet("GetAll")]
        public async Task<GeneralResponse<List<PageContentDto>>> GetAll(string? PageType, int? Count)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _pageContentService.GetAll(PageType, Count, isEnglish);
        }
        [HttpGet("GetById")]
        public async Task<GeneralResponse<PageContentDto>> GetById(Guid Id)
        {

            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _pageContentService.GetByIdAsync(Id, isEnglish);
        }



        [HttpPost("Add")]
        public async Task<GeneralResponse<Guid>> Add(PageContentInput input)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _pageContentService.Add(input, userId);
        }

        [HttpPost("Update")]
        public async Task<GeneralResponse<Guid>> Update(PageContentUpdateInput input)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _pageContentService.Update(input, userId);
        }
        [HttpPost("Delete")]
        public async Task<GeneralResponse<Guid>> Delete(Guid Id)
        {
            return await _pageContentService.SoftDelete(Id);
        }
        [HttpPost("SoftRangeDelete")]
        public async Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> id)
        {
            return await _pageContentService.SoftRangeDelete(id);
        }
    }
}
